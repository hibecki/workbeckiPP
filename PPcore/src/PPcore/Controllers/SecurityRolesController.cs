using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using PPcore.Models;
using System.Data.SqlClient;
using PPcore.ViewModels.SecurityRoles;
using Microsoft.AspNetCore.Http;

namespace PPcore.Controllers
{
    public class SecurityRolesController : Controller
    {
        private readonly SecurityDBContext _scontext;
        private readonly PalangPanyaDBContext _context;

        public SecurityRolesController(PalangPanyaDBContext context, SecurityDBContext scontext)
        {
            _context = context;
            _scontext = scontext;    
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole([Bind("RoleName")] SecurityRoles securityRoles)
        {
            if (ModelState.IsValid)
            {
                securityRoles.RoleId = Guid.NewGuid();
                securityRoles.CreatedBy = new Guid(HttpContext.Session.GetString("memberId"));
                securityRoles.CreatedDate = DateTime.Now;
                securityRoles.EditedBy = new Guid(HttpContext.Session.GetString("memberId"));
                securityRoles.EditedDate = DateTime.Now;
                securityRoles.x_status = "Y";
                securityRoles.RoleName = securityRoles.RoleName.Trim();

                _scontext.Add(securityRoles);

                SecurityRoleMenus rm1 = new SecurityRoleMenus();
                rm1.RoleId = securityRoles.RoleId;
                rm1.MenuId = 951000;
                rm1.EditedBy = securityRoles.EditedBy;
                rm1.EditedDate = securityRoles.EditedDate;
                _scontext.Add(rm1);

                SecurityRoleMenus rm2 = new SecurityRoleMenus();
                rm2.RoleId = securityRoles.RoleId;
                rm2.MenuId = 951030;
                rm2.EditedBy = securityRoles.EditedBy;
                rm2.EditedDate = securityRoles.EditedDate;
                _scontext.Add(rm2);

                SecurityRoleMenus rm3 = new SecurityRoleMenus();
                rm3.RoleId = securityRoles.RoleId;
                rm3.MenuId = 951040;
                rm3.EditedBy = securityRoles.EditedBy;
                rm3.EditedDate = securityRoles.EditedDate;
                _scontext.Add(rm3);

                SecurityRoleMenus rm4 = new SecurityRoleMenus();
                rm4.RoleId = securityRoles.RoleId;
                rm4.MenuId = 951050;
                rm4.EditedBy = securityRoles.EditedBy;
                rm4.EditedDate = securityRoles.EditedDate;
                _scontext.Add(rm4);

                try
                {
                    await _scontext.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    //"Violation of UNIQUE KEY constraint 'UK_SecurityRoles'. Cannot insert duplicate key in object 'dbo.SecurityRoles'. The duplicate key value is (??????).\r\nThe statement has been terminated."
                    if (e.InnerException.Message.Contains("UNIQUE"))
                    {
                        return Json(new { result = "dup", RoleName = securityRoles.RoleName });
                    }
                    else
                    {
                        return Json(new { result = "fail" });
                    }
                }
                return Json(new { result = "success" });
            }
            return Json(new { result = "fail" });
        }

        [HttpPost]
        public async Task<IActionResult> ChangeRoleName(string roleId, string rolename)
        {
            var r = await _scontext.SecurityRoles.SingleOrDefaultAsync(rr => rr.RoleId == new Guid(roleId));
            if (r != null)
            {
                r.RoleName = rolename.Trim();
                r.EditedBy = new Guid(HttpContext.Session.GetString("memberId"));
                r.EditedDate = DateTime.Now;
                _scontext.Update(r);
                await _scontext.SaveChangesAsync();
                return Json(new { result = "success", rolename = r.RoleName });
            }
            else
            {
                return Json(new { result = "fail" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string roleId)
        {
            var r = await _scontext.SecurityRoles.SingleOrDefaultAsync(rr => rr.RoleId == new Guid(roleId));
            if (r != null)
            {
                var memberCount = _context.member.Where(mm => mm.mem_role_id == r.RoleId).Count();
                if (memberCount != 0)
                {
                    return Json(new { result = "fail" });
                }
                else
                {
                    var rms = _scontext.SecurityRoleMenus.Where(rr => rr.RoleId == r.RoleId).ToList();
                    _scontext.RemoveRange(rms);

                    _scontext.Remove(r);
                    await _scontext.SaveChangesAsync();
                    return Json(new { result = "success" });
                }
            }
            else
            {
                return Json(new { result = "fail" });
            }
        }

        public async Task<IActionResult> DetailsAsBlock()
        {
            List<memberViewModel> ms = new List<memberViewModel>();
            var srs = await _scontext.SecurityRoles.Where(srr => srr.x_status != "N").OrderBy(srr => srr.CreatedDate).ToListAsync();
            foreach (SecurityRoles sr in srs)
            {
                memberViewModel m = new memberViewModel();
                m.SecurityRoles = sr;
                
                m.memberCount = _context.member.Where(mm => mm.mem_role_id == sr.RoleId).Count();

                var roleId = sr.RoleId.ToString();
                if (roleId != "c5a644a2-97b0-40e5-aa4d-e2afe4cdf428") //Not Administrators
                {
                    if (roleId != "9a1a4601-f5ee-4087-b97d-d69e7f9bfd7e") //Not Operators
                    {
                        if (roleId != "17822a90-1029-454a-b4c7-f631c9ca6c7d") //Not Members
                        {
                            m.panelColorCSS = "panel-dashboard-yellow";
                        }
                        else { m.panelColorCSS = "panel-dashboard-green"; } //Members
                    }
                    else { m.panelColorCSS = "panel-primary"; } //Operators
                }
                else { m.panelColorCSS = "panel-dashboard-black"; } //Administrators

                ms.Add(m);
            }
            return View(ms);
        }

        public async Task<IActionResult> DetailsAsRoleMenu()
        {
            return View(await _scontext.SecurityRoles.Where(sr => sr.x_status != "N").OrderBy(sr => sr.CreatedDate).ToListAsync());
        }

        public async Task<IActionResult> Manage(string roleId)
        {
            roleId = roleId.Trim();
            var r = await _scontext.SecurityRoles.SingleOrDefaultAsync(rr => rr.RoleId == new Guid(roleId));
            if (r != null)
            {
                ViewBag.RoleName = r.RoleName;
                ViewBag.RoleId = r.RoleId.ToString();
                if (roleId != "c5a644a2-97b0-40e5-aa4d-e2afe4cdf428") //Not Administrators
                {
                    if (roleId != "9a1a4601-f5ee-4087-b97d-d69e7f9bfd7e") //Not Operators
                    {
                        if (roleId != "17822a90-1029-454a-b4c7-f631c9ca6c7d") //Not Members
                        {
                            ViewBag.Color = "panel-dashboard-yellow"; ViewBag.IsDeleteLocked = false;
                        } else { ViewBag.Color = "panel-dashboard-green"; ViewBag.IsDeleteLocked = true; } //Members
                    } else { ViewBag.Color = "panel-primary"; ViewBag.IsDeleteLocked = true; } //Operators
                } else { ViewBag.Color = "panel-dashboard-black"; ViewBag.IsDeleteLocked = true; } //Administrators

                ViewBag.CountMembers = _context.member.Where(mm => mm.mem_role_id == r.RoleId).Count();

                string rmsstring = "";
                var rms = _scontext.SecurityRoleMenus.Where(rmss => rmss.RoleId == r.RoleId).OrderBy(rmss => rmss.MenuId).ToList();
                foreach ( SecurityRoleMenus rm in rms)
                {
                    rmsstring += "|"+rm.MenuId;
                }

                string menuHtmlCB = ""; int leftgap = 30;
                int prevLevel = 0; var countMenu = 0;
                var menus = _scontext.SecurityMenus.OrderByDescending(me => me.MenuId).ToList();
                foreach (SecurityMenus menu in menus)
                {
                    countMenu++;
                    if (menu.HaveChild == 1)
                    {
                        menuHtmlCB = menuHtmlCB.Replace("_parentMenuId_", menu.MenuId.ToString());
                    }
                    if (menu.Level > prevLevel)
                    {
                        menuHtmlCB = menuHtmlCB.Replace("_parentMenuId_", "0");
                    }
                    prevLevel = menu.Level;

                    leftgap = menu.Level * 30;
                    if (countMenu < 4)
                    {
                        menuHtmlCB = "<div id='menu-" + menu.MenuId + "' class='rolemanage-cb-check' style='margin-left:" + leftgap + "px;color:gray' onclick='checkMenuFreeze(_parentMenuId_)'><i id='menucb-" + menu.MenuId + "' class='cb-size-18d fa fa-check-square-o'></i>&nbsp;&nbsp;" + menu.MenuName + "</div>" + menuHtmlCB;
                    }
                    else
                    {
                        if ((roleId == "17822a90-1029-454a-b4c7-f631c9ca6c7d") && ((menu.MenuId == 101021) || (menu.MenuId == 401071))) //Members
                        {
                            menuHtmlCB = "<div id='menu-" + menu.MenuId + "' class='rolemanage-cb-check' style='margin-left:" + leftgap + "px;color:gray' onclick='checkMenuFreeze(_parentMenuId_)'><i id='menucb-" + menu.MenuId + "' class='cb-size-18d fa fa-check-square-o'></i>&nbsp;&nbsp;" + menu.MenuName + "</div>" + menuHtmlCB;
                        }
                        else
                        {
                            if ((menu.MenuId != 101021) && (menu.MenuId != 401071))
                            {
                                if (rmsstring.IndexOf(menu.MenuId.ToString()) != -1)
                                {
                                    menuHtmlCB = "<div id='menu-" + menu.MenuId + "' class='rolemanage-cb-check' style='margin-left:" + leftgap + "px;' onclick='checkMenu(" + menu.MenuId + ",_parentMenuId_)'><i id='menucb-" + menu.MenuId + "' class='cb-size-18 fa fa-check-square-o'></i>&nbsp;&nbsp;" + menu.MenuName + "</div>" + menuHtmlCB;
                                }
                                else
                                {
                                    menuHtmlCB = "<div id='menu-" + menu.MenuId + "' class='rolemanage-cb-uncheck' style='margin-left:" + leftgap + "px;' onclick='checkMenu(" + menu.MenuId + ",_parentMenuId_)'><i id='menucb-" + menu.MenuId + "' class='cb-size-18 fa fa-square-o'></i>&nbsp;&nbsp;" + menu.MenuName + "</div>" + menuHtmlCB;
                                }
                            }
                            else
                            {
                                menuHtmlCB = "<div id='menu-" + menu.MenuId + "' class='rolemanage-cb-uncheck' style='margin-left:" + leftgap + "px;color:gray' ><i id='menucb-" + menu.MenuId + "' class='cb-size-18d fa fa-square-o'></i>&nbsp;&nbsp;" + menu.MenuName + "</div>" + menuHtmlCB;
                            }
                        }
                    }

                }
                ViewBag.RoleCheckBox = menuHtmlCB.Replace("_parentMenuId_", "0"); ;
            }
            return View();
        }
    }
}
