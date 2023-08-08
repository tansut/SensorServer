using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Security;
using Kalitte.Sensors.Utilities;


namespace Kalitte.Sensors.Security
{
    [Serializable, KnownType(typeof(char[])), DataContract]
    public class UserNameAuthenticationInformation : AuthenticationInformation, IDisposable
    {
        // Fields
        [NonSerialized]
        private SecureString password;
        [DataMember]
        private readonly string username;

        // Methods
        public UserNameAuthenticationInformation(string userName, SecureString password)
        {
            this.username = userName;
            this.password = password;
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        public bool Equals(UserNameAuthenticationInformation other)
        {
            if (string.Equals(this.UserName, other.UserName, StringComparison.InvariantCulture))
            {
                char[] otherArr = SecurityHelper.CharArrayFromSecureString(other.Password);
                return CollectionsHelper.CompareArrays(PasswordForXml, otherArr);
            }
            else return false;
        }

        public override bool Equals(Object obj)
        {
            if (obj == null) return base.Equals(obj);

            if (!(obj is UserNameAuthenticationInformation))
                throw new InvalidCastException("The 'obj' argument is not a UserNameAuthenticationInformation object.");
            else
                return Equals(obj as UserNameAuthenticationInformation);
        }




        protected virtual void Dispose(bool disposing)
        {
            if (disposing && (this.password != null))
            {
                this.password.Dispose();
            }
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append("<userNameAuthenticationInfo>");
            builder.Append("<username>");
            builder.Append(this.username);
            builder.Append("</username>");
            builder.Append("</userNameAuthenticationInfo>");
            return builder.ToString();
        }

        // Properties
        public SecureString Password
        {
            get
            {
                return this.password;
            }
        }

        [DataMember(Name = "password")]
        private char[] PasswordForXml
        {
            get
            {
                return SecurityHelper.CharArrayFromSecureString(this.password);
            }
            set
            {
                this.password = SecurityHelper.SecureStringFromCharArray(value);
            }
        }

        public string UserName
        {
            get
            {
                return this.username;
            }
        }
    }




}
