
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

using Sitecore.Owin.Authentication.Identity;
using Sitecore.Owin.Authentication.Services;
using Sitecore.SecurityModel.Cryptography;
using System;
using System.Linq;

namespace DockerExamples.SSO.Pipelines
{
    public class ExternalUserBuilder : DefaultExternalUserBuilder
    {
        public ExternalUserBuilder(ApplicationUserFactory applicationUserFactory, IHashEncryption hashEncryption) : base(applicationUserFactory, hashEncryption)
        {
        }

        public override ApplicationUser BuildUser(UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            ApplicationUser user = this.ApplicationUserFactory.CreateUser(this.CreateUniqueUserName(userManager, externalLoginInfo));
            user.IsVirtual = false;
            user.Email = externalLoginInfo.Email;
            return user;
        }

        protected override string CreateUniqueUserName(UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            return base.CreateUniqueUserName(userManager, externalLoginInfo);
        }
    }
}