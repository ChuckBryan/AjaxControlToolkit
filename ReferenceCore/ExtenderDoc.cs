﻿using AjaxControlToolkit.Reference.Core.Rendering;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace AjaxControlToolkit.Reference.Core {

    public class ExtenderDoc {
        IDocRenderer _renderer;
        StringBuilder _docStringBuilder = new StringBuilder();

        public ExtenderDoc(IDocRenderer renderer) {
            _renderer = renderer;
        }

        public string BuildDoc(IEnumerable<TypeDoc> typeDocs) {
            var sb = new StringBuilder();

            foreach(var typeDoc in typeDocs)
                sb.Append(BuildTypeDoc(typeDoc));

            return sb.ToString();
        }

        public string BuildTypeDoc(TypeDoc typeDoc) {
            _docStringBuilder.Clear();

            RenderTypeName(typeDoc.Name);
            RenderTypeDescription(typeDoc.Summary);
            RenderTypeRemarks(typeDoc.Remarks);
            RenderMethods(typeDoc.Methods.OrderBy(m => m.Name), "Methods");
            RenderEvents(typeDoc.Events.OrderBy(e => e.Name));
            RenderProperties(typeDoc.Properties.OrderBy(p => p.Name));
            RenderClientProperties(typeDoc.ClientProperties.OrderBy(p => p.Name));
            RenderMethods(typeDoc.ClientMethods.OrderBy(m => m.Name), "Client methods");
            RenderClientEvents(typeDoc.ClientEvents.OrderBy(p => p.Name));

            return _docStringBuilder.ToString();
        }

        void RenderTypeRemarks(string typeRemarks) {
            _docStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(typeRemarks)));
        }

        void RenderTypeName(string typeName) {
            _docStringBuilder.Append(_renderer.RenderHeader(typeName));
        }

        void RenderTypeDescription(string typeDescription) {
            _docStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(typeDescription)));
        }

        void RenderMethods(IEnumerable<MethodDoc> methods, string headerText) {
            var methodsStringBuilder = new StringBuilder();
            methodsStringBuilder.Append(_renderer.RenderHeader(headerText, level: 2));

            foreach(var methodDoc in methods) {
                methodsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(methodDoc.Name, bold: true));
                methodsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(methodDoc.Summary)));
                methodsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(methodDoc.Remarks)));

                methodsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText("Params:", italic: true));

                foreach(var item in FormatMethodParams(methodDoc))
                    methodsStringBuilder.Append(_renderer.RenderLineBreak() + item);
            }

            _docStringBuilder.Append(methodsStringBuilder.ToString());
        }

        IEnumerable<string> FormatMethodParams(MethodDoc methodDoc) {
            foreach(var param in methodDoc.Params) {
                var header = _renderer.RenderText(param.Name, bold: true);
                var type = _renderer.RenderText("Type: ", italic: true) + _renderer.RenderText(param.TypeName);

                yield return
                    _renderer.RenderListItem(header, false, 1) +
                    _renderer.RenderListItem(type, false, 2) +
                    _renderer.RenderListItem(_renderer.Sanitize(param.Description), false, 2);
            }
        }

        void RenderEvents(IEnumerable<EventDoc> events) {
            var eventsStringBuilder = new StringBuilder();
            eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderHeader("Events", level: 2));

            foreach(var eventDoc in events) {
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(eventDoc.Name, bold: true));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(eventDoc.Summary)));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(eventDoc.Remarks)));
            }

            _docStringBuilder.Append(eventsStringBuilder.ToString());
        }

        void RenderClientProperties(IEnumerable<ClientPropertyDoc> clientProperties) {
            var propertiesStringBuilder = new StringBuilder();
            propertiesStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderHeader("Client properties", level: 2));

            foreach(var clientPropertyDoc in clientProperties) {
                propertiesStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(clientPropertyDoc.Name, bold: true));
                propertiesStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(clientPropertyDoc.Summary)));

                if(!String.IsNullOrWhiteSpace(clientPropertyDoc.GetterName))
                    propertiesStringBuilder.Append(
                        _renderer.RenderLineBreak() +
                        _renderer.RenderText("Getter name: ", bold: true) +
                        _renderer.RenderText(clientPropertyDoc.GetterName));

                if(!String.IsNullOrWhiteSpace(clientPropertyDoc.SetterName))
                    propertiesStringBuilder.Append(
                        _renderer.RenderLineBreak() +
                        _renderer.RenderText("Setter name: ", bold: true) +
                        _renderer.RenderText(clientPropertyDoc.SetterName));
            }

            _docStringBuilder.Append(propertiesStringBuilder.ToString());
        }

        void RenderProperties(IEnumerable<PropertyDoc> properties) {
            var propertiesStringBuilder = new StringBuilder();
            propertiesStringBuilder.Append(_renderer.RenderHeader("Properties", level: 2));

            foreach(var propertyDoc in properties) {
                propertiesStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(propertyDoc.Name, bold: true));
                propertiesStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(propertyDoc.Summary)));
                propertiesStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(propertyDoc.Remarks)));
            }

            _docStringBuilder.Append(propertiesStringBuilder.ToString());
        }

        void RenderClientEvents(IEnumerable<ClientEventDoc> clientEvents) {
            var eventsStringBuilder = new StringBuilder();
            eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderHeader("Client events", level: 2));

            foreach(var eventDoc in clientEvents) {
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(eventDoc.Name, bold: true));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(eventDoc.Summary)));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText(_renderer.Sanitize(eventDoc.Remarks)));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText("Add event handler method: ", bold: true) + _renderer.RenderText(eventDoc.AddMethodName));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText("Remove event handler method: ", bold: true) + _renderer.RenderText(eventDoc.RemoveMethodName));
                eventsStringBuilder.Append(_renderer.RenderLineBreak() + _renderer.RenderText("Raise event method: ", bold: true) + _renderer.RenderText(eventDoc.RaiseMethodName));
            }

            _docStringBuilder.Append(eventsStringBuilder.ToString());
        }

    }
}
