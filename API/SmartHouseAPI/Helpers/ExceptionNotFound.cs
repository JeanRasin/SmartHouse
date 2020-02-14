using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Runtime.Serialization;
using System.Text.Json;

namespace SmartHouseAPI.Helpers
{
    [Serializable()]
    public class NotFoundException : Exception
    {
        public NotFoundException() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, Exception inner) : base(message, inner) { }

        protected NotFoundException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

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
