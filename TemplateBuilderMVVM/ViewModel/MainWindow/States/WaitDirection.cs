﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using TemplateBuilder.Helpers;
using TemplateBuilder.Model;

namespace TemplateBuilder.ViewModel.MainWindow.States
{
    public class WaitDirection : Templating
    {
        private MinutiaRecord m_Record;

        public WaitDirection(TemplateBuilderViewModel outer, StateManager stateMgr) : base(outer, stateMgr)
        { }

        public override bool IsMinutiaTypeButtonsEnabled { get { return true; } }

        #region Overriden Public Methods

        public override void OnEnteringState()
        {
            base.OnEnteringState();
            m_Record = Outer.Minutae.Last();
            IntegrityCheck.IsNotNull(m_Record.Location);
        }

        public override void PositionMove(Point p)
        {
            // Update the direction whenever the mouse moves.
            SetDirection(p);
        }

        public override void PositionInput(Point p)
        {
            // The user has just finalised the direction of the minutia.
            SetDirection(p);
            StateMgr.TransitionTo(typeof(WaitLocation));
        }

        public override void RemoveMinutia(int index)
        {
            //Do nothing.
        }

        public override void SaveTemplate()
        {
            Logger.Debug("Cannot save template when waiting on direction.");
        }

        public override void SetMinutiaType(MinutiaType type)
        {
            // Update minutia type as user has changed it.
            m_Record.Type = Outer.InputMinutiaType;
        }

        public override void EscapeAction()
        {
            // Cancel adding the current minutia.
            Outer.Minutae.Remove(m_Record);
            StateMgr.TransitionTo(typeof(WaitLocation));
        }

        public override void MoveMinutia(int index, Point point)
        {
            throw IntegrityCheck.Fail(
                "It should not be possible to drag Minutia in the WaitDirection state.");
        }

        #endregion

        #region Private Methods

        private void SetDirection(Point p)
        {
            // Get the relevant record
            Vector direction = p - m_Record.Location.Scale(Outer.Scale);
            double angle = Math.Atan2(direction.Y, direction.X);
            // Save the new direction
            m_Record.Direction = angle;
        }

        public override void StartMove()
        {
            // The user may click the minutia when setting direction but this shouldn't start a
            // move!
        }

        public override void StopMove()
        {
            throw IntegrityCheck.Fail("Unexpected StopMove() call in {0} state.", GetType().Name);
        }

        #endregion
    }
}
