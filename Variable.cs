using System;

namespace WellByWellReview {
    public class Variable
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PropertyName { get; set; }
        public string InnerPropertyName { get; set; }
        public string FullPropertyName { get; set; }
        public Type DataType { get; set; }
        public bool PickFromInnerProperty { get; set; }
        public string AssetCategory { get; set; }
    }
}
