﻿using Xunit;
using Fusion1.License.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fusion1.License;
using Fusion1.License.Entity;

namespace Fusion1.License.Validation.Tests
{
    public class LicenseValidationTests
    {
        [Fact()]
        public void ParseLicenseFromBASE64StringTest()
        {
            var license = new Resource();
            var licenseString = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0idXRmLTE2Ij8+PExpY2Vuc2VFbnRpdHkgeG1sbnM6eHNpPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYS1pbnN0YW5jZSIgeG1sbnM6eHNkPSJodHRwOi8vd3d3LnczLm9yZy8yMDAxL1hNTFNjaGVtYSIgeHNpOnR5cGU9IkxpY2Vuc2UiPjxBcHBOYW1lPkRlbW9XaW5Gb3JtQXBwPC9BcHBOYW1lPjxVVUlEPjE0MTlBS0wtOVpTOEdQLVc5R1Y4Uy0xS1UwMUNSPC9VVUlEPjxDcmVhdGVEYXRlVGltZT4yMDIyLTAxLTIyVDExOjU1OjIyLjg3OTg0NCsxMDowMDwvQ3JlYXRlRGF0ZVRpbWU+PENvbm5lY3Rpb25zPjU8L0Nvbm5lY3Rpb25zPjxTaWduYXR1cmUgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyMiPjxTaWduZWRJbmZvPjxDYW5vbmljYWxpemF0aW9uTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvVFIvMjAwMS9SRUMteG1sLWMxNG4tMjAwMTAzMTUiIC8+PFNpZ25hdHVyZU1ldGhvZCBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDEvMDQveG1sZHNpZy1tb3JlI3JzYS1zaGEyNTYiIC8+PFJlZmVyZW5jZSBVUkk9IiI+PFRyYW5zZm9ybXM+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIiAvPjwvVHJhbnNmb3Jtcz48RGlnZXN0TWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMS8wNC94bWxlbmMjc2hhMjU2IiAvPjxEaWdlc3RWYWx1ZT5lVzI0dm1SOElXNEtmYTdyUUp2c1F3dzU2S1A3Mk1od1BFYVhJL2R2OTFZPTwvRGlnZXN0VmFsdWU+PC9SZWZlcmVuY2U+PC9TaWduZWRJbmZvPjxTaWduYXR1cmVWYWx1ZT5rNDJaSjE1V0J2ckxWN2ttNjJqUGVHanZkckNHR0lueWVkQXNoeEkzblNYSDM3SHJ1UU9uaG1BMjJVT1hBVzN0cGRZL3VMSTVvc2dwQldyZ2FiSHBqQVdxUU1ZcEF1QzRFRkpDVlRrbnpCb1grSCtCdTJNbW9ONHBKWFFoRWhIcXV6NmZuQzYrWUJta1hhTjJqd1NMWWljWGpjbVVpSy9GWUdQeGw1dEVvelVtZU1jc1JOTjhRRXo1YXVTMmU2OTI2c3k1UmREOVRlK2lhZHYxTmFIQ2lGVmlmVkMyNFd2Q2dPZmg0Q2QzYS9nd1U2SkRxaDF3bGM1VkJ4RFRFb256d2w4YWZZSzIxb09tM3IwQlNsZzZMTXdiaFZBVkVRc1hMSXREYnpHaGRqK3RObmVxZVB4NjhXTjIyT0ZDekZQb3Jjbnc2NG42enlpY0NjY0YvdE9BdHc9PTwvU2lnbmF0dXJlVmFsdWU+PC9TaWduYXR1cmU+PC9MaWNlbnNlRW50aXR5Pg==";
            var licStatus = LicenseStatus.UNDEFINED;
            var validationMsg = "";
            license = (Resource)LicenseValidation.ParseLicenseFromBASE64String(typeof(Resource),licenseString, out licStatus, out validationMsg);
            Assert.Equal(LicenseStatus.VALID, licStatus);
        }
    }
}