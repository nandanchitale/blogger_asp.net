using Helpers.Constants;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using Npgsql;
using NS_callLogService.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helpers.Helper
{
    public class CommonOperations
    {
        private readonly ILogger<CommonOperations> _logger;
        private readonly IConfiguration _configuration;
        public CommonOperations(ILogger<CommonOperations> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        /// <summary>
        /// Method to get the next sequence number value from database
        /// </summary>
        /// <returns></returns>
        public long GetNextSeqNumber()
        {
            long seq = 0;
            try
            {
                using NpgsqlConnection conn = new NpgsqlConnection(_configuration.GetConnectionString("jediConnection"));
                conn.Open();
                using NpgsqlCommand cmd = new NpgsqlCommand(DBProcedures.Proc_Get_Next_Seq_No, conn);
                seq = (long)cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {

                // Log error to console and create a new response we can return to the requesting method
                _logger.LogError($"Exception at CommonServices > GetNextSeqNumber()");
                _logger.LogError($"Unhandled Exception. ID: {ex.StackTrace} - Message: {ex.Message}");
            }
            return seq;
        }
    }
}
