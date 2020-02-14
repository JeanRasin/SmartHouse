using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Runtime.Serialization;
using System.Text.Json;

namespace SmartHouseAPI.ApiException
{
    [Serializable()]
    public class ModelStateException : Exception
    {
        public ModelStateException() { }

        public ModelStateException(string message) : base(message) { }

        public ModelStateException(string message, ModelStateDictionary modelState) : base($"{message}:+ {JsonSerializer.Serialize(modelState)}")
        {
        }

        public ModelStateException(string message, Exception inner) : base(message, inner) { }

        protected ModelStateException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
