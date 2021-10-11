using System;
using System.Collections.Generic;

namespace WellByWellReview.contract {
    public interface IVariableService {
        IEnumerable<Variable> GetAll();
        IEnumerable<Variable> GetAll(string assetCategory);
        IEnumerable<Variable> GetAll(Predicate<Variable> searchPredicate);
        IEnumerable<Variable> GetAll(IEnumerable<string> Ids);
        Variable Get(string id);
        void AddVariable(Variable variable);
        void AddVariables(IEnumerable<Variable> variables);
        void RemoveVariable(string Id);
        void RemoveVariables(IEnumerable<string> Ids);
        void GenerateVariables(Type assemblyReference);
        void Save();
        void Load();
    }
}
