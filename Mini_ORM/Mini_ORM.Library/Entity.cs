using System;
using System.Collections.Generic;
using System.Text;

namespace Mini_ORM.Library
{
    public interface  IEntity<TKey>
    {
        TKey Id { get; set; }
    }
}
