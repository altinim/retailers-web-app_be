using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Core.Exceptions {
    public class DuplicateEmailException : Exception {
        public DuplicateEmailException() {
        }

        public DuplicateEmailException(string? message) : base(message) {
        }

        public DuplicateEmailException(string? message, Exception? innerException) : base(message, innerException) {
        }

        protected DuplicateEmailException(SerializationInfo info, StreamingContext context) : base(info, context) {
        }
    }
}
