using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace Prabesh_Academy.Modules.Views
{
   

    public class HierarchicalContent
    {
        public int ContentId { get; set; }
        public int SubjectId { get; set; }
        public int? ParentId { get; set; }
        public string ContentType { get; set; }
        public string ContentName { get; set; }
        public string ContentDetails { get; set; }
    }

    public class ContentRetriever
    {
        public List<HierarchicalContent> GetContentBySubject(int subjectId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["pAcademyCourses"].ConnectionString;
            List<HierarchicalContent> contentList = new List<HierarchicalContent>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT content_id, subject_id, parent_id, content_type, content_name, content_details " +
                               "FROM HierarchicalContent WHERE subject_id = @subjectId ORDER BY parent_id, content_id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@subjectId", subjectId);

                SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    contentList.Add(new HierarchicalContent
                    {
                        ContentId = reader.GetInt32(0),
                        SubjectId = reader.GetInt32(1),
                        ParentId = reader.IsDBNull(2) ? (int?)null : reader.GetInt32(2),
                        ContentType = reader.GetString(3),
                        ContentName = reader.GetString(4),
                        ContentDetails = reader.IsDBNull(5) ? null : reader.GetString(5)
                    });
                }
            }

            return contentList;
        }
    }

}
