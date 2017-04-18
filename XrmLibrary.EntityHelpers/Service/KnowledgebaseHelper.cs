using System;
using System.ComponentModel;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Sasha.Exceptions;
using XrmLibrary.EntityHelpers.Common;

namespace XrmLibrary.EntityHelpers.Service
{
    /// <summary>
    /// A <c>Knowledge Base (Article)</c> is a type of structured content that is managed internally as part of a knowledge base. 
    /// With the <c>Knowledge Base (Article)</c>, you can manage the distribution of product and service information for a business unit. 
    /// This class provides mostly used common methods for <c>KbArticle</c> entity.
    /// <para>
    /// For more information look at https://msdn.microsoft.com/en-us/library/gg309378(v=crm.7).aspx
    /// </para>
    /// </summary>
    public class KnowledgebaseHelper : BaseEntityHelper
    {
        #region | Bookmarks |

        /*
         * INFO : Bookmarks.Knowledgebase
         * http://crmbook.powerobjects.com/basics/service-management-overview/knowledge-base/
         * https://community.dynamics.com/crm/b/crmcat/archive/2015/07/12/how-to-use-articles-in-microsoft-dynamics-crm-amp-why-you-should-use-them 
         */

        #endregion

        #region | Enums |

        public enum SearchTypeCode
        {
            ByBody = 1,
            ByKeyword = 2,
            ByTitle = 3
        }

        /// <summary>
        /// <c>KbArticle</c> 's statecode values
        /// <para>
        /// For more information look at https://technet.microsoft.com/en-us/library/dn531157.aspx#BKMK_Article
        /// </para>
        /// </summary>
        public enum KbArticleStateCode
        {
            [Description("draft")]
            Draft = 1,

            [Description("unapproved")]
            Unapproved = 2,

            [Description("published")]
            Published = 3
        }

        /// <summary>
        /// <c>KbArticle</c> 's <c>Draft</c> statuscode values
        /// <para>
        /// For more information look at https://technet.microsoft.com/en-us/library/dn531157.aspx#BKMK_Article
        /// </para>
        /// </summary>
        public enum KbArticleDraftStatusCode
        {
            CustomStatusCode = 0,
            Draft = 1
        }

        /// <summary>
        /// <c>KbArticle</c> 's <c>Unapproved</c> statuscode values
        /// <para>
        /// For more information look at https://technet.microsoft.com/en-us/library/dn531157.aspx#BKMK_Article
        /// </para>
        /// </summary>
        public enum KbArticleUnapprovedStatusCode
        {
            CustomStatusCode = 0,
            Unapproved = 2
        }

        /// <summary>
        /// <c>KbArticle</c> 's <c>Published</c> statuscode values
        /// <para>
        /// For more information look at https://technet.microsoft.com/en-us/library/dn531157.aspx#BKMK_Article
        /// </para>
        /// </summary>
        public enum KbArticlePublishedStatusCode
        {
            CustomStatusCode = 0,
            Published = 3
        }

        #endregion

        #region | Constructors |

        /// <summary>
        /// Default constructor
        /// </summary>
        /// <param name="service"><see cref="IOrganizationService"/></param>
        public KnowledgebaseHelper(IOrganizationService service) : base(service)
        {
            this.SdkHelpUrl = "https://msdn.microsoft.com/en-us/library/gg309378(v=crm.7).aspx";
            this.EntityName = "kbarticle";
        }

        #endregion

        #region | Public Methods |

        /// <summary>
        /// Publish <c>Article</c>.
        /// </summary>
        /// <param name="id"><c>Article</c> Id</param>
        public void Publish(Guid id)
        {
            ExceptionThrow.IfGuidEmpty(id, "id");

            CommonHelper commonHelper = new CommonHelper(this.OrganizationService);
            commonHelper.UpdateState(id, this.EntityName, (int)KbArticleStateCode.Published, (int)KbArticlePublishedStatusCode.Published);
        }

        /// <summary>
        /// Unpublish <c>Article</c>.
        /// </summary>
        /// <param name="id"><c>Article</c> Id</param>
        public void UnPublish(Guid id)
        {
            ExceptionThrow.IfGuidEmpty(id, "id");

            CommonHelper commonHelper = new CommonHelper(this.OrganizationService);
            commonHelper.UpdateState(id, this.EntityName, (int)KbArticleStateCode.Unapproved, (int)KbArticleUnapprovedStatusCode.Unapproved);
        }

        /// <summary>
        /// Retrieve <c>Article</c> 's content by <c>Html</c>.
        /// </summary>
        /// <param name="id"><c>Article</c> Id</param>
        /// <returns>
        /// <c>Html</c> data for <c>Article</c> content.
        /// </returns>
        public string RetrieveContent(Guid id)
        {
            ExceptionThrow.IfGuidEmpty(id, "id");

            string result = string.Empty;

            var data = this.Get(id, "content");

            ExceptionThrow.IfNull(data, "KbArticle");

            result = data.GetAttributeValue<string>("content");
            return result;
        }

        /// <summary>
        /// Retrieve <c>Article</c> 's <c>Subject</c>.
        /// </summary>
        /// <param name="id"></param>
        /// <returns>
        /// <c>Subject</c> Id (<see cref="System.Guid"/>)
        /// </returns>
        public Guid RetrieveSubjectId(Guid id)
        {
            ExceptionThrow.IfGuidEmpty(id, "id");

            var data = this.Get(id, "subjectid");

            ExceptionThrow.IfNull(data, "KbArticle");
            ExceptionThrow.IfNull(data.GetAttributeValue<EntityReference>("subjectid"), "KbArticle.SubjectId");

            return data.GetAttributeValue<EntityReference>("subjectid").Id;
        }

        /// <summary>
        /// Retrieve the top 10 <c>Articles</c> about a specified product.
        /// <para>
        /// For more information look at https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.retrievebytopincidentproductkbarticlerequest(v=crm.7).aspx
        /// </para>
        /// </summary>
        /// <param name="id">Product Id</param>
        /// <returns>
        /// <see cref="EntityCollection"/> for <c>Article</c> data
        /// </returns>
        public EntityCollection RetrieveByTopIncidentProduct(Guid id)
        {
            ExceptionThrow.IfGuidEmpty(id, "id");

            RetrieveByTopIncidentProductKbArticleRequest request = new RetrieveByTopIncidentProductKbArticleRequest()
            {
                ProductId = id
            };

            var serviceResponse = (RetrieveByTopIncidentProductKbArticleResponse)this.OrganizationService.Execute(request);
            return serviceResponse.EntityCollection;
        }

        /// <summary>
        /// Retrieve the top 10 <c>Articles</c> about a specified subject.
        /// <para>
        /// For more information look at https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.retrievebytopincidentsubjectkbarticlerequest(v=crm.7).aspx
        /// </para>
        /// </summary>
        /// <param name="id">Subject Id</param>
        /// <returns>
        /// <see cref="EntityCollection"/> for <c>Article</c> data
        /// </returns>
        public EntityCollection RetrieveByTopIncidentSubject(Guid id)
        {
            ExceptionThrow.IfGuidEmpty(id, "id");

            RetrieveByTopIncidentSubjectKbArticleRequest request = new RetrieveByTopIncidentSubjectKbArticleRequest()
            {
                SubjectId = id
            };

            var serviceResponse = (RetrieveByTopIncidentSubjectKbArticleResponse)this.OrganizationService.Execute(request);
            return serviceResponse.EntityCollection;
        }

        /// <summary>
        /// Search for <c>Knowledge Base Articles</c>.
        /// <para>
        /// For more information look at 
        /// <c>By Body</c> : https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbybodykbarticlerequest(v=crm.7).aspx
        /// <c>By Keyword</c> : https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbykeywordskbarticlerequest(v=crm.7).aspx
        /// <c>By Title</c> : https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbytitlekbarticlerequest(v=crm.7).aspx
        /// </para>
        /// </summary>
        /// <param name="searchType"><see cref="SearchTypeCode"/></param>
        /// <param name="subjectId">Knowledge base article subject Id</param>
        /// <param name="searchText">Text contained in the body of the article</param>
        /// <param name="useInflection">Indicates whether to use inflectional stem matching when searching for knowledge base articles with a specified body text</param>
        /// <returns>
        /// <see cref="EntityCollection"/> for <c>Article</c> data
        /// </returns>
        public EntityCollection Search(SearchTypeCode searchType, Guid subjectId, string searchText, bool useInflection)
        {
            ExceptionThrow.IfGuidEmpty(subjectId, "subjectId");
            ExceptionThrow.IfNullOrEmpty(searchText, "searchText");

            QueryExpression query = new QueryExpression(this.EntityName)
            {
                ColumnSet = new ColumnSet(true)
            };

            return Search(searchType, query, subjectId, searchText, useInflection);
        }

        /// <summary>
        /// Search for <c>Knowledge Base Articles</c>.
        /// <para>
        /// For more information look at 
        /// <c>By Body</c> : https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbybodykbarticlerequest(v=crm.7).aspx
        /// <c>By Keyword</c> : https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbykeywordskbarticlerequest(v=crm.7).aspx
        /// <c>By Title</c> : https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbytitlekbarticlerequest(v=crm.7).aspx
        /// </para>
        /// </summary>
        /// <param name="searchType"><see cref="SearchTypeCode"/></param>
        /// <param name="query">
        /// Query criteria to find knowledge base articles with specified keyword.
        /// This parameter supports <c>QueryExpression</c> and <c>FetchXml</c>.
        /// </param>
        /// <param name="subjectId">Knowledge base article subject Id</param>
        /// <param name="searchText">Text contained in the body of the article</param>
        /// <param name="useInflection">Indicates whether to use inflectional stem matching when searching for knowledge base articles with a specified body text</param>
        /// <returns>
        /// <see cref="EntityCollection"/> for <c>Article</c> data
        /// </returns>
        public EntityCollection Search(SearchTypeCode searchType, QueryBase query, Guid subjectId, string searchText, bool useInflection)
        {
            ExceptionThrow.IfNull(query, "query");
            ExceptionThrow.IfGuidEmpty(subjectId, "subjectId");
            ExceptionThrow.IfNullOrEmpty(searchText, "searchText");

            EntityCollection result = new EntityCollection();

            if (query is QueryExpression)
            {
                ExceptionThrow.IfNullOrEmpty(((QueryExpression)query).EntityName, "QueryExpression.EntityName");
            }

            OrganizationRequest request;
            OrganizationResponse serviceResponse;

            switch (searchType)
            {
                case SearchTypeCode.ByBody:
                    //INFO : SDK Url --> https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbybodykbarticlerequest(v=crm.7).aspx

                    request = new SearchByBodyKbArticleRequest()
                    {
                        QueryExpression = query,
                        SearchText = searchText,
                        SubjectId = subjectId,
                        UseInflection = useInflection
                    };

                    serviceResponse = this.OrganizationService.Execute(request);
                    result = ((SearchByBodyKbArticleResponse)serviceResponse).EntityCollection;

                    break;

                case SearchTypeCode.ByKeyword:
                    //INFO : SDK Url --> https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbykeywordskbarticlerequest(v=crm.7).aspx

                    request = new SearchByKeywordsKbArticleRequest()
                    {
                        QueryExpression = query,
                        SearchText = searchText,
                        SubjectId = subjectId,
                        UseInflection = useInflection
                    };

                    serviceResponse = this.OrganizationService.Execute(request);
                    result = ((SearchByKeywordsKbArticleResponse)serviceResponse).EntityCollection;

                    break;

                case SearchTypeCode.ByTitle:
                    //INFO : SDK Url --> https://msdn.microsoft.com/en-us/library/microsoft.crm.sdk.messages.searchbytitlekbarticlerequest(v=crm.7).aspx

                    request = new SearchByTitleKbArticleRequest()
                    {
                        QueryExpression = query,
                        SearchText = searchText,
                        SubjectId = subjectId,
                        UseInflection = useInflection
                    };

                    serviceResponse = this.OrganizationService.Execute(request);
                    result = ((SearchByTitleKbArticleResponse)serviceResponse).EntityCollection;

                    break;
            }

            return result;
        }

        #endregion
    }
}
