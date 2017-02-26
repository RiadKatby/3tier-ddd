using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core.Workflow
{
    /// <summary>
    /// Represent only information about file or attachment which is releated to a Specific <see cref="Request"/>
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Gets identity number of <see cref="Attachment"/> object.
        /// </summary>
        public int AttachmentId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="Request"/> which has this <see cref="Attachment"/>.
        /// </summary>
        public int RequestId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="User"/> who create this <see cref="Attachment"/>.
        /// </summary>
        public int CreatedById { get; private set; }

        /// <summary>
        /// Gets Reference id of Blob content at <see cref="FileService"/>.
        /// </summary>
        public Guid FileReference { get; private set;}

        /// <summary>
        /// Gets the <see cref="User"/> who update this <see cref="Attachment"/> to <see cref="Request"/>.
        /// </summary>
        public User CreatedBy { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Attachment"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Attachment() { }

        /// <summary>
        /// Instanciate custom <see cref="Attachment"/> object.
        /// </summary>
        /// <param name="createdBy"><see cref="User"/> who attach this <see cref="Attachment"/> to <see cref="Request"/>.</param>
        public Attachment(User createdBy)
        {
            this.CreatedBy = createdBy;
            this.CreatedById = createdBy.UserId;
        }

        /// <summary>
        /// Attach blob content data from FileService to this <see cref="Attachment"/> object.
        /// </summary>
        /// <param name="blob">byte buffer content of this <see cref="Attachment"/>.</param>
        /// <param name="Url">specific Url path to actual file of this <see cref="Attachment"/>.</param>
        /// <returns>Current instance of <see cref="Attachment"/> object.</returns>
        public Attachment Attach(byte[] blob, string Url)
        {
            // Those parameter must came from File Service, and must be populated in this class
            return this;
        }
    }
}
