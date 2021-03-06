﻿using DataModels.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace DataModels.Dao
{
   public class UserDao
    {
        OnlineShopingDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopingDbContext();
        }

        public long Insert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }

        public long InsertForFacebook (User entity)
        {
            var user = db.Users.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user ==null)
            {
                db.Users.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }
         
        }
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                user.Name = entity.Name;
                user.UserName = entity.UserName;
                if (!string .IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.ModifiedDate = DateTime.Now;
                user.Status = entity.Status;
                db.SaveChanges();
                return true;

            }
            catch (Exception )
            {

                return false;
            }

        }
        // this method to get all users
        public IEnumerable<User> listAllPaging(string searchString,int page ,int pageSize )
        {
            IQueryable<User> model = db.Users;
            if (!string .IsNullOrEmpty (searchString))
            {
                model = model.Where(x => x.UserName.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page,pageSize);
        }

        public User GetById (string userName )
        {
            return db.Users.SingleOrDefault(x=>x.UserName==userName);
        }

        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }


        public int Login(string userName,string passWord)
        {
            var result = db.Users.SingleOrDefault(x => x.UserName == userName);
            if(result ==null )
            {
                return 0 ;
            }
            else
            {
                if (result.Status==false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                        return 1;
                    else
                        return -2;
                }
            }

        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        //this method to change status by ajax with json data
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);

            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }

        public bool CheckUserName (string userName)
        {
            return db.Users.Count(x => x.UserName == userName) >0;
        }

        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}
