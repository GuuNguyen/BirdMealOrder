﻿using BusinessObject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DAOs
{
    public class UserDAO
    {
        public static List<User> GetUsers()
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static User GetUser(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    return context.Users.Find(id);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Create(User user)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    context.Users.Add(user);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Delete(int id)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var m = context.Users.Find(id);
                    context.Remove(m);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public static void Update(User userUpdate)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    context.Users.Update(userUpdate);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static bool isExitedPhoneNumber(string phoneNumer)
        {
            try
            {
                using(var context = new BirdMealOrderDBContext())
                {
                    var check = context.Users.Any(u => u.PhoneNumber == phoneNumer);
                    if (check)
                    {
                        return true;
                    }
                    return false;
                }
            }catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        public static bool isExitedUserName(string userName)
        {
            try
            {
                using (var context = new BirdMealOrderDBContext())
                {
                    var check = context.Users.Any(u => u.UserName == userName);
                    if (check)
                    {
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}