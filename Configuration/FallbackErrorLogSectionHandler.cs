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
    /// <para>Replaces the legacy <see cref="Elmah.ErrorLogSectionHandler"/> to allow multiple child nodes under "errorLog" section.</para>
    /// <para>If the "type" attribute of the section is "FallbackErrorLog", an instance of "CompositeErrorLogSection" will be returned.
    /// Otherwise, returns a result based on <see cref="SingleTagSectionHandler"/>.</para>
    /// </summary>
    /// 
    public class FallbackErrorLogSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            var typeAttr = section.Attributes["type"];
            var typeSpec = typeAttr != null ? typeAttr.Value : null;

            if (string.IsNullOrEmpty(typeSpec) 
                || !typeSpec.StartsWith(typeof(FallbackErrorLog).FullName))
            {
                // for compatibility with Elmah.ErrorLogSectionHandler:
                return new SingleTagSectionHandler().Create(parent, configContext, section);
            }

            return new CompositeErrorLogSection(typeSpec, section.ChildNodes.Cast<XmlNode>());
        }
    }
}
