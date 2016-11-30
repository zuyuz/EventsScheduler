﻿using EventsScheduler.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventsScheduler
{
    /// <summary>
    /// Represents control unit to manage business logic
    /// Singleton
    /// </summary>
    public class Controller
    {
        private static Controller instance;

        private User currentUser;

        private Controller()
        {
        }

        public User CurrentUser 
        {
            get
            {
                return this.currentUser;
            }
        }

        /// <summary>
        /// Identificates, authentificates and authorizates user 
        /// by login and password
        /// </summary>
        /// <param name="login">user's login</param>
        /// <param name="password">user's password</param>
        /// <returns>whether user is signed in</returns>
        public async Task<bool> SignInAsync(string login, string password)
        {
            return await Task.Run(() =>
            {
                using (var dataManager = new UnitOfWork(new AppDbContext()))
                {
                    User user = dataManager.Users.GetUserByLogin(login);
                    if (user != null) // identification
                    {
                        if (user.Password == password) // authentification
                        {
                            currentUser = user; // authorization
                            return true;
                        }
                    }
                }

                return false;
            });
        }

        public void SignOut()
        {
            this.currentUser = null;
        }

        public string GetCurrentUserLogin()
        {
            return this.currentUser.Login;
        }

		/// <summary>
		/// Performs registration and updates DB.
		/// </summary>
		/// <param name="email">User email</param>
		/// <param name="name">User full name</param>
		/// <param name="login">User login</param>
		/// <param name="password">User password</param>
		/// <returns>Whether registration completed successfully</returns>
		public async Task<bool> RegisterUserAsync(
			string email, 
			string name, 
			string login, 
			string password)
		{
            return await Task.Run(() =>
            {
                using (var dataManager = new UnitOfWork(new AppDbContext()))
                {
                    if (dataManager.Users.GetUserByLogin(login) == null)
                    {
                        User registrant = new User();
                        registrant.Login = login;
                        registrant.Name = name;
                        registrant.Password = password;
                        registrant.UserRole = User.Role.User;

                        dataManager.Users.Add(registrant);
                        //remove completion if necessary
                        dataManager.Complete();

                        return true;
                    }

                    return false;
                }
            });
		}

        public  void CreateEventAsync(
            string name,
            DateTime begin,
            string beginTimeStr,
            DateTime end,
            string endTimeStr,
            string freePlacesStr,
            string locationStr,
            string creatorLogin,
			List<User> participants)
		{
				using (var dataManager = new UnitOfWork(new AppDbContext()))
				{
                    TimeSpan beginTime = new TimeSpan();
                    TimeSpan endTime = new TimeSpan();
                    if(TimeSpan.TryParseExact(beginTimeStr, @"hh\:mm", null, out beginTime) == false
                    || TimeSpan.TryParseExact(endTimeStr, @"hh\:mm", null, out endTime) == false)
                    {
                        throw new ArgumentException("Invalid time!\nTime format is HH:MM");
                    }
                    begin = begin.Add(beginTime);
                    end = end.Add(endTime);
                    if(begin > end)
                    {
                        throw new ArgumentException("Invalid period of time");
                    }

                    int freePlaces;
                    if (int.TryParse(freePlacesStr, out freePlaces) == false)
                    {
                        throw new ArgumentException("Invalid number of participants!");
                    }

                    if (dataManager.Events.GetEventsInSpecificPeriod(begin, end).Count() > 0)
                    {
                        throw new ArgumentException("Event in such period already exists.");
                    }

                    Location location = dataManager.Locations.GetLocationByAddress(locationStr);

                    Event createdEvent = new Event()
						{
							Name = name,
							StartTime = begin,
							EndTime = end,
							FreePlaces = freePlaces,
							EventLocation = location,
							Creator = dataManager.Users.GetUserByLogin(creatorLogin),
							Participants = participants
						};
						dataManager.Events.Add(createdEvent);

						dataManager.Complete();
				}
		}

		public async Task<bool> AddLocationAsync(string address)
        {
			return await Task.Run(() =>
			{
				using (var dataManager = new UnitOfWork(new AppDbContext()))
				{
					if (dataManager.Locations.GetLocationByAddress(address) == null)
					{
						Location location = new Location();
						location.Address = address;

						dataManager.Locations.Add(location);

						dataManager.Complete();

						return true;
					}
					else
					{
						return false;
					}
				}
			});
        }

        /// <summary>
        /// Getter for singleton instance of <c>Controller</c> type
        /// </summary>
        public static Controller Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Controller();
                }

                return instance;
            }
        }
    }
}
