using System.Collections.Generic;

namespace IUV.SDN
{
    public interface IFileServer<T>
    {
        void Save(List<T> list);
        List<T> Load();
    }
}