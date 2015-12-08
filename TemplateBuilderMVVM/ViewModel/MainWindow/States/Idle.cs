﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using TemplateBuilder.Helpers;
using TemplateBuilder.Model;

namespace TemplateBuilder.ViewModel.MainWindow.States
{
    public class Idle : Initialised
    {
        public Idle(TemplateBuilderViewModel outer, StateManager stateMgr) : base(outer, stateMgr)
        { }

        #region Overriden Abstract Methods

        public override void OnEnteringState()
        {
            base.OnEnteringState();

            // Deactivate UI controls.
            Outer.IsSaveTemplatePermitted = false;
            Outer.IsInputMinutiaTypePermitted = false;

            // Hide old image from UI, and remove other things.
            Outer.Image = null;
            Outer.Minutae.Clear();

            LoadImage();
        }

        public override void PositionInput(Point point)
        {
            // Do nothing.
        }

        public override void PositionMove(Point point)
        {
            // Do nothing.
        }

        public override void RemoveMinutia(int index)
        {
            // There should be no minutia visible in this state.
            // TODO: Transition to Error?
            throw IntegrityCheck.Fail(
                "There should be no minutia visible in this state.");
        }

        public override void SaveTemplate()
        {
            // The save template button should be deactivated in this state.
            // TODO: Transition to error?
            throw IntegrityCheck.Fail(
                "The save template button should be deactivated in the Idle state.");
        }

        public override void SetMinutiaType(MinutiaType type)
        {
            // No record to update.
        }

        public override void EscapeAction()
        {
            // Nothing to escape.
        }
        public override void MoveMinutia(int index, Point point)
        {
            throw IntegrityCheck.Fail(
                "It should not be possible to drag Minutia in the Idle state.");
        }

        public override void StartMove()
        {
            // Do nothing.
        }

        public override void StopMove()
        {
            // Do nothing.
        }

        #endregion
    }
}
