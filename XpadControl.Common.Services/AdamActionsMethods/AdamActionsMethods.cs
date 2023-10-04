using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using XpadControl.Interfaces;
using XpadControl.Interfaces.AdamActionsMethodsDependencies;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies;
using XpadControl.Interfaces.WebSocketClientsService.Dependencies.JsonModel;

namespace XpadControl.Common.Services.AdamActionsMethods
{
    /// <summary>
    /// Command implementations are defined here
    /// The conditions under which they are called are defined in the base class
    /// </summary>
    public class AdamActionsMethods : AdamActionsMethodsBase, IAdamActionsMethods
    {
        private readonly ILoggerService mLoggerService;
        private readonly IWebSocketClientsService mWebSocketClients;
        private readonly ServoCommands mHomePositionCommands;
        private readonly int mNeckZeroPosition;

        public AdamActionsMethods(ILoggerService loggerService, IWebSocketClientsService webSocketClients, string zeroPositionConfigPath)
        {
            mLoggerService = loggerService;
            mWebSocketClients = webSocketClients;
            mHomePositionCommands = zeroPositionConfigPath.ToServoCommands();

            mNeckZeroPosition = mHomePositionCommands.Motors.Where(x => x.Name == ServoNames.Neck).Select(x => x.GoalPosition).FirstOrDefault();
        }

        #region ToHomePosition

        public override void HomePositionCommandExecute()
        {
            WriteDebubLog();
            mWebSocketClients.SendInstant(mHomePositionCommands);
        }

        #endregion

        #region HeadUpDown/HeadUp/HeadDown

        public override void HeadUpDown(float value)
        {
            WriteDebubLog();

            int zeroPositions = mNeckZeroPosition;
            int newGoalPosition = value.ToServoRange(zeroPositions);

            ServoCommands command = new()
            {
                Motors = new List<ServoCommandsItem>
                {
                    new ServoCommandsItem
                    {
                        Name = ServoNames.Neck,
                        GoalPosition = newGoalPosition
                    }
                }
            };

            mWebSocketClients.SendInstant(command);
        }

        #region HeadUpDown

        #endregion

        #region HeadUp

        public override void HeadUp(int value)
        {
            WriteDebubLog();

            ServoCommands command = new()
            {
                Motors = new List<ServoCommandsItem>
                {
                    new ServoCommandsItem
                    {
                        Name = ServoNames.Neck,
                        GoalPosition = mNeckZeroPosition + value
                    } 
                }
            };

            mWebSocketClients.SendInstant(command);
        }

        public override void HeadUp(bool value)
        {
            while (value) 
            {
                ServoCommands command = new()
                {
                    Motors = new List<ServoCommandsItem>
                    {
                        new ServoCommandsItem
                        {
                            Name = ServoNames.Neck,
                            GoalPosition = mNeckZeroPosition + 1
                        }
                    }
                };

                mWebSocketClients.SendInstant(command);
            }
        }

        #endregion

        #region HeadDown

        public override void HeadDown(int value)
        {
            WriteDebubLog();

            ServoCommands command = new()
            {
                Motors = new List<ServoCommandsItem>
                {
                    new ServoCommandsItem
                    {
                        Name = ServoNames.Neck,
                        GoalPosition = mNeckZeroPosition - value
                    }
                }
            };

            mWebSocketClients.SendInstant(command);
        }

        public override void HeadDown(bool value)
        {
            WriteDebubLog();

            while (value)
            {
                ServoCommands command = new()
                {
                    Motors = new List<ServoCommandsItem>
                    {
                        new ServoCommandsItem
                        {
                            Name = ServoNames.Neck,
                            GoalPosition = mNeckZeroPosition - 1
                        }
                    }
                };

                mWebSocketClients.SendInstant(command);
            }
        }

        #endregion

        #endregion


        private void WriteDebubLog([CallerMemberName] string callerMember = "")
        {
            mLoggerService.WriteDebugLog($"AdamActionsMethods called {callerMember}");
        }
    }
}
