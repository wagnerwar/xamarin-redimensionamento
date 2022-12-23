using System;
using System.Collections.Generic;
using System.Text;

namespace Redimensionamento.Interface
{
    public interface IPathService
    {
        string Pictures { get; }
        byte[] ResizeImageAndroid(byte[] imageData, float width, float height);
    }
}
