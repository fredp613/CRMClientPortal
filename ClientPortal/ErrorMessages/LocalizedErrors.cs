using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Web;

namespace ClientPortal.ErrorMessages
{

    [Description]
    public enum LocalizedErrorMessages
    {
        // The flag for SunRoof is 0001.
        [Description("Organization already exists")]
        AccountExistsEN,
        [Description("Organisation existe deja")]
        AccountExistsFR       
    }
   
}