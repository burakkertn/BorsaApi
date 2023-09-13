using BorsaApi.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BorsaApi.DataAccessLayer
{
    public class BorsaApiDatabaseSettings : IBorsaApiDatabaseSettings
    {

        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
		public string UserCollectionName { get; set; }
	}
}
