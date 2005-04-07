// Copyright 2004-2005 Castle Project - http://www.castleproject.org/
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//     http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

namespace Castle.CastleOnRails.Framework.Views.CompositeView
{
	using System;
	using System.IO;
	using System.Web;
	using System.Collections;

	using Castle.CastleOnRails.Framework.Views.Aspx;
	using Castle.CastleOnRails.Framework.Views.NVelocity;

	/// <summary>
	/// Composition of view engines that dispatch to 
	/// one or other based on the view file extesion.
	/// </summary>
	public class CompositeViewEngine : IViewEngine
	{
		private String _viewRootDir;
		private AspNetViewEngine _aspxViewEngine;
		private NVelocityViewEngine _nvelocityViewEngine;

		public CompositeViewEngine()
		{
		}

		private void InitViews()
		{
			_aspxViewEngine = new AspNetViewEngine();
			_aspxViewEngine.ViewRootDir = _viewRootDir;

			_nvelocityViewEngine = new NVelocityViewEngine();
			_nvelocityViewEngine.ViewRootDir = _viewRootDir;
		}

		#region IViewEngine Members

		public String ViewRootDir
		{
			get { return _viewRootDir; }
			set
			{
				_viewRootDir = value;
				InitViews();
			}
		}

		public void Process(IRailsEngineContext context, Controller controller, String viewName)
		{
			FileInfo aspxFile = new FileInfo(Path.Combine( _viewRootDir, viewName + ".aspx" ));
			FileInfo vmFile = new FileInfo(Path.Combine( _viewRootDir, viewName + ".vm" ));
			
			if (aspxFile.Exists)
			{
				_aspxViewEngine.Process(context, controller, viewName);
			}
			else if (vmFile.Exists)
			{
				_nvelocityViewEngine.Process(context, controller, viewName);
			}
			else
			{
				String message = String.Format("No view file (aspx or vm) found for {0}", viewName);
				throw new RailsException(message);
			}
		}

		#endregion
	}
}
