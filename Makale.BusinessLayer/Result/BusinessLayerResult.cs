﻿using Makale.Entities.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Makale.BusinessLayer.Result
{
    public class BusinessLayerResult<T> where T : class
    {
        public List<ErrorMessageObj> Errors { get; set; }
        public T Result { get; set; }

        public BusinessLayerResult()
        {
            Errors = new List<ErrorMessageObj>();
        }

        public void AddError(ErrorMessagesCode code,string message)
        {
            Errors.Add(new ErrorMessageObj() {Code=code, Message=message });
 
        }

        internal void AddError(object userCouldNotFind, string v)
        {
            throw new NotImplementedException();
        }
    }
}
