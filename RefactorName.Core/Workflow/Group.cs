using RefactorName.GraphDiff.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RefactorName.Core
{
    /// <summary>
    /// Group is a collection of Users that perform related functions.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Gets identity number of <see cref="Group"/> object.
        /// </summary>
        public int GroupId { get; private set; }

        /// <summary>
        /// Gets identity number of the <see cref="Process"/> which has this <see cref="Group"/>.
        /// </summary>
        public int ProcessId { get; private set; }
        [Associated]
        public Process Process { get; private set; }

        /// <summary>
        /// Gets the name of this <see cref="Group"/>.
        /// </summary>
        [Required, StringLength(100)]
        public string Name { get; private set; }

        /// <summary>
        /// Gets list of <see cref="User"/>s that are Members of this <see cref="Group"/> object.
        /// </summary>
        [Associated]
        public IList<User> Users { get; private set; }

        //[Associated]
        //public IList<Action> Actions { get; private set; }

        public IList<ActionTarget> ActionTargets { get; private set; }

        public IList<ActivityTarget> ActivityTargets { get; private set; }

        /// <summary>
        /// Instanciate empty <see cref="Group"/> object, this constructor used by infrastrcutre libraries only.
        /// </summary>
        public Group()
        {
            this.Users = new List<User>();
        }

        /// <summary>
        /// Instanciate custom <see cref="Group"/> object.
        /// </summary>
        /// <param name="name">name of <see cref="Group"/></param>
        public Group(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Modify <see cref="Group"/> object.
        /// </summary>
        /// <param name="name">new name of <see cref="Group"/>.</param>
        /// <returns>Current instance of <see cref="Group"/> object.</returns>
        public Group Update(string name)
        {
            this.Name = name;

            return this;
        }

        /// <summary>
        /// Add new <see cref="User"/> member to this <see cref="Group"/>.
        /// </summary>
        /// <param name="member">member <see cref="User"/>.</param>
        /// <returns>Current instance of <see cref="Group"/> object.</returns>
        public Group AddMember(User member)
        {
            var result = (from user in this.Users where user.Id == member.Id select user).ToList();
            if (result.Count == 0)
                this.Users.Add(member);

            return this;
        }

        /// <summary>
        /// Delete <see cref="User"/> member from this <see cref="Group"/>.
        /// </summary>
        /// <param name="member">member <see cref="User"/>.</param>
        /// <returns>Current instance of <see cref="Group"/> object.</returns>
        public Group DeleteMember(User member)
        {
            this.Users.Remove(member);

            return this;
        }

        public Group RemoveMember(User member)
        {
            this.Users.Remove(member);
            return this;
        }

        public Group UpdateName(string name)
        {
            this.Name = name;
            return this;
        }
    }
}
