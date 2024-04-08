using System;
using UnityEngine;
using UnityEngine.Events;

namespace AdsManager
{
	public class AdsManagerBS : MonoBehaviour {

		public string privacyPolicyLink;

		#region AdMob
		[Header("Admob")]
		[SerializeField] private string adMobAppID = "";
		[SerializeField] private string interstitalAdMobId = "";
		[SerializeField] private string videoAdMobId = "";
		//private InterstitialAd interstitialAdMob;
		
		//private RewardBasedVideoAd rewardBasedAdMobVideo; 
		//private AdRequest requestAdMobInterstitial, adMobVideoRequest;
		#endregion
		
		[Space(15)]
		#region
		[Header("UnityAds")]
		private string unityAdsGameId;
		private string unityAdsVideoPlacementId = "rewardedVideo";
		#endregion

		private static AdsManagerBS _instance;

		[HideInInspector()]
		public UnityEvent videoRewarded;

		public static AdsManagerBS Instance
		{
			get
			{
				if(_instance == null)
					_instance = GameObject.FindObjectOfType(typeof(AdsManagerBS)) as AdsManagerBS;
			
				return _instance;
			}
		}

		private void Awake ()
		{
			gameObject.name = GetType().Name;
			DontDestroyOnLoad(gameObject);
			InitializeAds();
			videoRewarded ??= new UnityEvent();
		}

		public void ShowInterstitial()
		{
			//ShowAdMob();
		}

		public void IsVideoRewardAvailable()
		{
			//if(isVideoAvaiable())
			//{
			
			//}
			//else
			//{

			//}
		}

		//bool isVideoAvaiable()
		//{
		//if(Advertisement.IsReady(unityAdsVideoPlacementId))
		//{
		//    return true;
		//}
		//else if(rewardBasedAdMobVideo.IsLoaded())
		//{
		//    return true;
		//}
		//return false;
		//}

		public void ShowVideoReward()
		{
		
        
			//if (Advertisements.Instance.IsRewardVideoAvailable())
			//{
				//Implementation.Instance.ShowRewardedVideo();
			//}
			//else
			{
				GameObject.Find("Canvas").GetComponent<MenuManagerBS>().ShowPopUpMessage("VIDEO NOT READY", "PLEASE TRY AGAIN LATER");
			}
		}

		private void RequestInterstitial()
		{
			//// Initialize an InterstitialAd.
			//interstitialAdMob = new InterstitialAd(interstitalAdMobId);

			//// Called when an ad request has successfully loaded.
			//interstitialAdMob.OnAdLoaded += HandleOnAdLoaded;
			//// Called when an ad request failed to load.
			//interstitialAdMob.OnAdFailedToLoad += HandleOnAdFailedToLoad;
			//// Called when an ad is shown.
			//interstitialAdMob.OnAdOpening += HandleOnAdOpened;
			//// Called when the ad is closed.
			//interstitialAdMob.OnAdClosed += HandleOnAdClosed;
			//// Called when the ad click caused the user to leave the application.
			//interstitialAdMob.OnAdLeavingApplication += HandleOnAdLeavingApplication;

			//// Create an empty ad request.
			//requestAdMobInterstitial = new AdRequest.Builder().Build();
			//// Load the interstitial with the request.
			//interstitialAdMob.LoadAd(requestAdMobInterstitial);
		}

		public void ShowAdMob()
		{
			//if(interstitialAdMob.IsLoaded())
			//{
			//	interstitialAdMob.Show();
			//}
			//else
			//{
			//	interstitialAdMob.LoadAd(requestAdMobInterstitial);
			//}
		}

		public void HandleOnAdLoaded(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleAdLoaded event received");
		}

		public void HandleOnAdFailedToLoad(object sender)
		{
			//MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.Message);
		}

		public void HandleOnAdOpened(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleAdOpened event received");
		}

		public void HandleOnAdClosed(object sender, EventArgs args)
		{
			//MonoBehaviour.print("HandleAdClosed event received");
			//interstitialAdMob.LoadAd(requestAdMobInterstitial);
		}

		public void HandleOnAdLeavingApplication(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleAdLeftApplication event received");
		}

		private void RequestRewardedVideo()
		{
			//// Called when an ad request has successfully loaded.
			//rewardBasedAdMobVideo.OnAdLoaded += HandleRewardBasedVideoLoadedAdMob;
			//// Called when an ad request failed to load.
			//rewardBasedAdMobVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoadAdMob;
			//// Called when an ad is shown.
			//rewardBasedAdMobVideo.OnAdOpening += HandleRewardBasedVideoOpenedAdMob;
			//// Called when the ad starts to play.
			//rewardBasedAdMobVideo.OnAdStarted += HandleRewardBasedVideoStartedAdMob;
			//// Called when the user should be rewarded for watching a video.
			//rewardBasedAdMobVideo.OnAdRewarded += HandleRewardBasedVideoRewardedAdMob;
			//// Called when the ad is closed.
			//rewardBasedAdMobVideo.OnAdClosed += HandleRewardBasedVideoClosedAdMob;
			//// Called when the ad click caused the user to leave the application.
			//rewardBasedAdMobVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplicationAdMob;
			//// Create an empty ad request.
			//AdMobVideoRequest = new AdRequest.Builder().Build();
			//// Load the rewarded video ad with the request.
			//this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
		}

		public void HandleRewardBasedVideoLoadedAdMob(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoLoaded event received");
		
		}

		public void HandleRewardBasedVideoFailedToLoadAdMob(object sender)
		{
			//MonoBehaviour.print("HandleRewardBasedVideoFailedToLoad event received with message: " + args.Message);

		}

		public void HandleRewardBasedVideoOpenedAdMob(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoOpened event received");
		}

		public void HandleRewardBasedVideoStartedAdMob(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoStarted event received");
		}

		public void HandleRewardBasedVideoClosedAdMob(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoClosed event received");
			//this.rewardBasedAdMobVideo.LoadAd(AdMobVideoRequest, videoAdMobId);
		}

		public void HandleRewardBasedVideoRewardedAdMob(object sender)
		{
			//string type = args.Type;
			//double amount = args.Amount;
			//MonoBehaviour.print("HandleRewardBasedVideoRewarded event received for " + amount.ToString() + " " + type);
			videoRewarded.Invoke();
		}

		public void HandleRewardBasedVideoLeftApplicationAdMob(object sender, EventArgs args)
		{
			MonoBehaviour.print("HandleRewardBasedVideoLeftApplication event received");
		}

		void InitializeAds()
		{
			//MobileAds.Initialize(adMobAppID);
			//this.rewardBasedAdMobVideo = RewardBasedVideoAd.Instance;
			//this.RequestRewardedVideo();
			//Advertisement.Initialize(unityAdsGameId);
			//RequestInterstitial();
		}
		
		private void AdMobShowVideo()
		{
			//rewardBasedAdMobVideo.Show();	
		}

		private void UnityAdsShowVideo()
		{
			//ShowOptions options = new ShowOptions();
			//options.resultCallback = HandleShowResultUnity;

			//Advertisement.Show(unityAdsVideoPlacementId, options);
		}

		//void HandleShowResultUnity (ShowResult result)
		//{
		//	if(result == ShowResult.Finished) {
		//		Debug.Log("Video completed - Offer a reward to the player");
		//           videoRewarded.Invoke();
		//		Advertisement.Initialize(unityAdsGameId);
		//	}else if(result == ShowResult.Skipped) {
		//		Debug.LogWarning("Video was skipped - Do NOT reward the player");

		//	}else if(result == ShowResult.Failed) {
		//		Debug.LogError("Video failed to show");
		//	}
		//}

//	bool isVideoAvaiable()
//	{
//		#if !UNITY_EDITOR
//		if(Advertisement.IsReady(unityAdsVideoPlacementId))
//		{
//			return true;
//		}
//		else if(rewardBasedAdMobVideo.IsLoaded())
//		{
//			return true;
//		}
//		#endif
//		return false;
//	}
	}
}
