using System;
using System.Runtime.Serialization;

namespace RelatedCustomLookup
{
    [DataContract, Serializable]
    public class BaseDataContract : IExtensibleDataObject
    {
        [NonSerialized]
        private ExtensionDataObject _extensionDataObject;

        public ExtensionDataObject ExtensionData
        {
            get
            {
                return _extensionDataObject;
            }
            set
            {
                _extensionDataObject = value;
            }
        }
    }
}