using System;
using System.ComponentModel.DataAnnotations;

namespace TicketManagement.WebMVC.Atributtes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Method | AttributeTargets.Parameter, AllowMultiple = false)]
    public class PasswordLocalizeAttribute : ValidationAttribute
    {
        private static string[] _dataTypeStrings = Enum.GetNames(typeof(DataType));

        public PasswordLocalizeAttribute(DataType dataType)
        {
            DataType = dataType;
        }

        public PasswordLocalizeAttribute(string customDataType)
            : this(DataType.Custom)
        {
            CustomDataType = customDataType;
        }

        public DataType DataType { get; private set; }

        public string CustomDataType { get; private set; }

        public DisplayFormatAttribute DisplayFormat { get; protected set; }

        public virtual string GetDataTypeName()
        {
            EnsureValidDataType();

            return DataType == DataType.Custom ? CustomDataType : _dataTypeStrings[(int)DataType];
        }

        public override bool IsValid(object value)
        {
            EnsureValidDataType();

            return true;
        }

        private void EnsureValidDataType()
        {
            if (DataType == DataType.Custom && string.IsNullOrEmpty(CustomDataType))
            {
                throw new InvalidOperationException("DataTypeAttribute_EmptyDataTypeString");
            }
        }
    }
}
