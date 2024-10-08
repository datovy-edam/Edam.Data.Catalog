﻿using Edam.Application;
using System.Diagnostics;

namespace Edam.Data.CatalogService;

public class WebAppService : IDisposable
{
   public WebApplication? Application { get; }
   public CatalogSystem CatalogSystem { get; } = new();

   private static string? _sessionId;
   public static string SessionId
   {
      get { return _sessionId; }
   }

   public static bool IsInitialized { get; private set; }

   public WebAppService(WebApplication? application)
   {
      if (application == null) 
         throw new ArgumentNullException(nameof(application));
      Application = application;
   }

   /// <summary>
   /// If needed setup session ID.
   /// </summary>
   /// <param name="sessionId"></param>
   public static void SetupSession(string sessionId)
   {
      if (!IsInitialized)
      {
         _sessionId = sessionId;
         IsInitialized = true;
      }
   }

   /// <summary>
   /// Release resources.
   /// </summary>
   public void Dispose()
   {
      if (CatalogSystem != null)
      {
      }
   }

}
