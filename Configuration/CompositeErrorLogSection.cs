using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Xml;

namespace Elmah
{
    /// <summary>
    /// <para>Represents the configuration section "errorLog" that can contain child nodes.</para>
    /// <para>Instance of this object should be constructed by <see cref="FallbackErrorLogSectionHandler"/>.</para>
    /// </summary>
    public class CompositeErrorLogSection
        : IDictionary, ICloneable // interfaces used by Elmah.SimpleServiceProviderFactory.CreateFromConfigSection
    {
        private readonly Dictionary<string, IDictionary> _innerLogErrorElements;

        private readonly string _compositeType;

       

        /// <summary>
        /// Constructor based on child nodes of the "errorLog" section. Each node must be an "add" element.
        /// </summary>
        /// <param name="compositeType">Name of the composite type, typically "Elmah.FallbackErrorLog, Elmah.FallbackErrorLog."</param>
        /// <param name="errorLogs">Child nodes (similar to &lt;add name="sql" type="Elmah.SqlErrorLog, Elmah" [...] /&gt;).</param>
        public CompositeErrorLogSection(string compositeType, IEnumerable<XmlNode> errorLogs)
        {
            if (string.IsNullOrEmpty(compositeType))
                throw new ArgumentNullException("compositeType");
            if (errorLogs == null)
                throw new ArgumentNullException("errorLogs");

            _compositeType = compositeType;
            _innerLogErrorElements = new Dictionary<string, IDictionary>(2);
            XmlAttribute attr;
            foreach (var node in errorLogs)
            {
                if (node.Name != "add")
                    throw new ConfigurationErrorsException("Unexpected node: " + node.Name);

                if ((attr = node.Attributes["type"]) == null)
                    throw new ConfigurationErrorsException("Attribute 'type' is missing.");

                var typeSpec = attr.Value;
                var name = (attr = node.Attributes["name"]) == null ? typeSpec : attr.Value;
                Hashtable config = new Hashtable(node.Attributes.Count);
                foreach (XmlAttribute attribute in node.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case "name":
                            break;
                        //case "type":
                        default:
                            config.Add(attribute.Name, attribute.Value);
                            break;
                    }
                }
                _innerLogErrorElements.Add(name, config);
            }
        }

        #region IDictionary
        public void Add(object key, object value)
        {
            ((IDictionary)_innerLogErrorElements).Add(key, value);
        }

        public void Clear()
        {
            _innerLogErrorElements.Clear();
        }

        public bool Contains(object key)
        {
            return ((IDictionary)_innerLogErrorElements).Contains(key);
        }

        public IDictionaryEnumerator GetEnumerator()
        {
            return _innerLogErrorElements.GetEnumerator();
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public bool IsReadOnly
        {
            get { return true; }
        }

        public ICollection Keys
        {
            get { return _innerLogErrorElements.Keys; }
        }

        public void Remove(object key)
        {
            ((IDictionary)_innerLogErrorElements).Remove(key);
        }

        public ICollection Values
        {
            get { return _innerLogErrorElements.Values; }
        }

        public object this[object key]
        {
            get
            {
                if (key == "type")
                    return _compositeType;
                return ((IDictionary)_innerLogErrorElements)[key];
            }
            set
            {
                ((IDictionary)_innerLogErrorElements)[key] = value;
            }
        }

        public void CopyTo(Array array, int index)
        {
            ((IDictionary)_innerLogErrorElements).CopyTo(array, index);
        }

        public int Count
        {
            get { return _innerLogErrorElements.Count; }
        }

        public bool IsSynchronized
        {
            get { return ((IDictionary)_innerLogErrorElements).IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return ((IDictionary)_innerLogErrorElements).SyncRoot; }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _innerLogErrorElements.GetEnumerator();
        } 
        #endregion

        #region IClonable
        public object Clone()
        {
            var copy = new Dictionary<string, IDictionary>(_innerLogErrorElements.Count);
            foreach (var kvp in _innerLogErrorElements)
                copy.Add(kvp.Key, kvp.Value);
            return new CompositeErrorLogSection(_compositeType, copy);
        }


        private CompositeErrorLogSection(string compositeType, Dictionary<string, IDictionary> elements)
        {
            _compositeType = compositeType;
            _innerLogErrorElements = elements;
        }
        #endregion
    }
}
