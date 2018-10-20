using System;
///sdcard/android/data/
namespace KJVSharp
{
    public class Base
    {
        public Base()
        {
           _oid= StaticHelperMethods.GetRandomOid();

        }
        private Guid _oid;

        public Guid Oid { get => _oid; private set => _oid = value; }
    }
}
