using System;
using System.Collections.Generic;

namespace WSE.Model {
    public class Field : ModelBase {
        public string Name { get; set; }
        public int OML { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public Client Client { get; set; }
        public IEnumerable<Well> Wells { get; set; }
        public IEnumerable<int> ListValues { get; set; }
    }
}
