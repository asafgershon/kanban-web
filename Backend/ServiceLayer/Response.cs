#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Kanban_2024_2024_24.Backend.BusinessLayer.TaskAndBoard;
using Kanban_2024_2024_24.Backend.BusinessLayer.User;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Response
    {
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public string? ErrorMessage { get; set; }

        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public object? ReturnValue { get; set; }

        [JsonIgnore]
        public bool isError { get => ErrorMessage != null; }

        //empty constructor
        public Response() { }

        //constructor with only error
        public Response(string errorMessage)
        {
            ErrorMessage = errorMessage;
            ReturnValue = null;
        }

        //constructor with error and return value - use (null,returnvalue) for return valid return value
        public Response(string errorMessage, object returnValue)
        {
            ErrorMessage = errorMessage;
            ReturnValue = returnValue;
        }

        //overide equal
        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            Response other = (Response)obj;
            bool errorMessageEqual = ErrorMessage == other.ErrorMessage || (ErrorMessage != null && ErrorMessage.Equals(other.ErrorMessage));
            bool returnValueEqual = ReturnValue == other.ReturnValue || (ReturnValue != null && ReturnValue.Equals(other.ReturnValue));
            return errorMessageEqual && returnValueEqual;
        }
    }
}