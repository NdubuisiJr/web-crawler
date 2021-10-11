using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using WellByWellReview.contract;

namespace WellByWellReview.service {
    public class VariableService : IVariableService {
        public VariableService() {
            //Todo - Ndubuisi change this path later
            FilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "variables.csv");
        }

        public void AddVariable(Variable variable) {
            _variables.Add(variable.Id, variable);
        }

        public void AddVariables(IEnumerable<Variable> variables) {
            foreach (var variable in variables) {
                AddVariable(variable);
            }
        }

        public void GenerateVariables(Type baseClass) {
            var types = baseClass.Assembly
                        .GetTypes()
                        .Where(x => x.IsSubclassOf(baseClass));
            foreach (var type in types) {
                var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                foreach (var property in properties) {
                    string typeName = property.DeclaringType.FullName.ToLower();

                    var variable = new Variable {
                        Id = Guid.NewGuid().ToString(),
                        PropertyName = property.Name,
                        FullPropertyName = $"{property.DeclaringType.FullName}.{property.Name}",
                        DataType = property.PropertyType,
                        AssetCategory = typeName.Contains("field") ? "field" :
                            typeName.Contains("reservoir") ? "reservoir" :
                            typeName.Contains("well") ? "well" :
                            typeName.Contains("drainage") ? "drainage-point" : 
                            "others",
                    };
                    AddVariable(variable);
                }
            }
        }

        public Variable Get(string id) {
            if (!_variables.ContainsKey(id))
                return null;
            return _variables[id];
        }

        public IEnumerable<Variable> GetAll() {
            return _variables.Values;
        }

        public IEnumerable<Variable> GetAll(string assetCategory) {
            return _variables.Values.Where(x => x.AssetCategory.ToLower() == assetCategory.ToLower());
        }

        public IEnumerable<Variable> GetAll(Predicate<Variable> searchPredicate) {
            return _variables.Values.Where(x=> searchPredicate(x));
        }

        public IEnumerable<Variable> GetAll(IEnumerable<string> ids) {
            foreach (var id in ids) {
                yield return Get(id);
            }
        }

        public void RemoveVariable(string id) {
            if (_variables.ContainsKey(id)) {
                _variables.Remove(id);
            }
        }

        public void RemoveVariables(IEnumerable<string> ids) {
            foreach (var id in ids) {
                RemoveVariable(id);
            }
        }

        public void Save() {
            var lines = new List<string>() { "Id, Name,PropertyName,FullPropertyName,AssetCategory,PickFromInnerProperty,InnerPropertyName,DataType" };
            foreach (var variable in _variables.Values) {
                lines.Add($"{variable.Id},{variable.Name},{variable.PropertyName},{variable.FullPropertyName},{variable.AssetCategory},{variable.PickFromInnerProperty},{variable.InnerPropertyName},{variable.DataType}");
            }
            File.WriteAllLines(FilePath, lines);
        }

        public void Load() {
            _variables.Clear();

            var lines = File.ReadAllLines(FilePath).Skip(1);
            foreach (var line in lines) {
                var data = line.Split(',');
                var variable = new Variable {
                    Id = data[0],
                    Name = data[1],
                    PropertyName = data[2],
                    FullPropertyName = data[3],
                    AssetCategory = data[4],
                    PickFromInnerProperty = bool.Parse(data[5]),
                    InnerPropertyName = data[6],
                    DataType = Type.GetType(data[7], true, true)
                };
                _variables.Add(variable.Id, variable);
            }
        }

        public string FilePath { get; }
        private Dictionary<string, Variable> _variables = new Dictionary<string, Variable>();
    }
}
