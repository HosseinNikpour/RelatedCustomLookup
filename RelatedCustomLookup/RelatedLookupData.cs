using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace RelatedCustomLookup
{
    [Serializable, DataContract]
public class RelatedLookupData : BaseDataContract
{
    // Methods
    public RelatedLookupData()
    {
        this.SavedDisplayValues = new Dictionary<int, string>();
    }

    // Properties
    [DataMember]
    public Guid DisplayId { get; set; }

    [DataMember]
    public Guid ListId { get; set; }

    [DataMember]
    public Dictionary<int, string> SavedDisplayValues { get; set; }

    [DataMember]
    public Guid ValueId { get; set; }
}


}
