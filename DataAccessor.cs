using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using WSE.Model;

namespace WellByWellReview {
    public class DataAccessor {
        /// <summary>
        /// Returns a single point value defined by @variable and contained in @model
        /// </summary>
        /// <param name="variable">The variable/data description</param>
        /// <param name="model">The model that contains the data</param>
        /// <returns></returns>
        public Response FetchSingle(Variable variable, ModelBase model) {
            try {
                var property = model.GetType()
                                    .GetProperties()
                                    .FirstOrDefault(x => 
                                        $"{x.DeclaringType.FullName}.{x.Name}" == variable.FullPropertyName
                                     );
                if (property == null)
                    return null;

                var value = property.GetValue(model);

                if (!string.IsNullOrWhiteSpace(variable.InnerPropertyName) && variable.PickFromInnerProperty) {
                    value = FetchSingle(
                        new Variable() {
                            PropertyName = variable.InnerPropertyName,
                            Name = variable.Name
                        },
                        (ModelBase)value
                    )?
                    .Value;
                }

                return new Response {
                    Label = variable.Name,
                    Value = value
                };
            }
            catch (Exception) {
                // Log Error
                throw;
            }
        }

        /// <summary>
        /// Returns all point values defined by @variable and contained in each model in @models
        /// </summary>
        /// <param name="variable">The particular variable to return</param>
        /// <param name="models">The models that contains the values</param>
        /// <returns></returns>
        public IEnumerable<Response> FetchSingle(Variable variable, IEnumerable<ModelBase> models) {
            foreach (var model in models) {
                yield return FetchSingle(variable, model);
            }
        }

        /// <summary>
        /// Returns all the point values specified in @variables and contained in @model
        /// </summary>
        /// <param name="variables">The variable descriptions</param>
        /// <param name="model">The model that contain the values</param>
        /// <returns></returns>
        public IEnumerable<Response> FetchSingle(IEnumerable<Variable> variables, ModelBase model) {
            foreach (var variable in variables) {
                yield return FetchSingle(variable, model);
            }
        }

        /// <summary>
        /// Returns all specified variables for all the models passed
        /// </summary>
        /// <param name="variables">The variable descriptions</param>
        /// <param name="models">The models that hold the values</param>
        /// <returns></returns>
        public IEnumerable<IEnumerable<Response>> FetchSingle(IEnumerable<Variable> variables, IEnumerable<ModelBase> models) {
            var result = new List<List<Response>>();
            foreach (var model in models) {
                var modelResults = new List<Response>();
                foreach (var variable in variables) {
                    modelResults.Add(FetchSingle(variable, model));
                }
                result.Add(modelResults);
            }
            return result;
        }

        /// <summary>
        /// Returns a list of values as described in the variable object
        /// </summary>
        /// <param name="variable">variable description for the values to fetch</param>
        /// <param name="model">the model that contains the valuesparam>
        /// <returns></returns>
        public Response FetchMultiple(Variable variable, ModelBase model) {
            try {
                var property = model.GetType()
                                    .GetProperties()
                                    .FirstOrDefault(x => 
                                        $"{x.DeclaringType.FullName}.{x.Name}" == variable.FullPropertyName
                                    );
                if (property == null)
                    return null;

                var values = property.GetValue(model) as IEnumerable;
                if (values is null)
                    return null;

                if (!string.IsNullOrWhiteSpace(variable.InnerPropertyName) && variable.PickFromInnerProperty) {
                    var list = new ArrayList();
                    foreach (var value in values) {
                        list.Add(
                          FetchSingle(
                              new Variable() {
                                  PropertyName = variable.InnerPropertyName,
                                  Name = variable.Name
                              },
                              (ModelBase)value
                          )?
                          .Value
                        );
                    }
                    values = list;
                }

                return new Response {
                    Label = variable.Name,
                    Value = values
                };
            }
            catch (Exception) {
                throw;
            }
        }
    }
}
