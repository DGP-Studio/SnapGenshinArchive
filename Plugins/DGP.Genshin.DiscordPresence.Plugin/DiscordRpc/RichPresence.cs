using Newtonsoft.Json;
/// <summary>
/// The Rich Presence structure that will be sent and received by Discord. Use this class to build your presence and update it appropriately.
/// </summary>
// This is broken up in this way because the response inherits the BaseRichPresence.
public sealed class RichPresence : BaseRichPresence
{
    /// <summary>
    /// The buttons to display in the presence. 
    /// <para>Max of 2</para>
    /// </summary>
    [JsonProperty("buttons", NullValueHandling = NullValueHandling.Ignore)]
    public Button[] Buttons { get; set; }

    /// <summary>
    /// Does the Rich Presence have any buttons?
    /// </summary>
    /// <returns></returns>
    public bool HasButtons()
    {
        return Buttons != null && Buttons.Length > 0;
    }


    #region Builder
    /// <summary>
    /// Sets the state of the Rich Presence. See also <seealso cref="BaseRichPresence.State"/>.
    /// </summary>
    /// <param name="state">The user's current <see cref="Party"/> status.</param>
    /// <returns>The modified Rich Presence.</returns>
    public RichPresence WithState(string state)
    {
        State = state;
        return this;
    }

    /// <summary>
    /// Sets the details of the Rich Presence. See also <seealso cref="BaseRichPresence.Details"/>.
    /// </summary>
    /// <param name="details">What the user is currently doing.</param>
    /// <returns>The modified Rich Presence.</returns>
    public RichPresence WithDetails(string details)
    {
        Details = details;
        return this;
    }

    /// <summary>
    /// Sets the timestamp of the Rich Presence. See also <seealso cref="Timestamps"/>.
    /// </summary>
    /// <param name="timestamps">The time elapsed / remaining time data.</param>
    /// <returns>The modified Rich Presence.</returns>
    public RichPresence WithTimestamps(Timestamps timestamps)
    {
        Timestamps = timestamps;
        return this;
    }

    /// <summary>
    /// Sets the assets of the Rich Presence. See also <seealso cref="Assets"/>.
    /// </summary>
    /// <param name="assets">The names of the images to use and the tooltips to give those images.</param>
    /// <returns>The modified Rich Presence.</returns>
    public RichPresence WithAssets(Assets assets)
    {
        Assets = assets;
        return this;
    }

    /// <summary>
    /// Sets the Rich Presence's party. See also <seealso cref="Party"/>.
    /// </summary>
    /// <param name="party">The party the player is currently in.</param>
    /// <returns>The modified Rich Presence.</returns>
    public RichPresence WithParty(Party party)
    {
        Party = party;
        return this;
    }

    /// <summary>
    /// Sets the Rich Presence's secrets. See also <seealso cref="Secrets"/>.
    /// </summary>
    /// <param name="secrets">The secrets used for Join / Spectate.</param>
    /// <returns>The modified Rich Presence.</returns>
    public RichPresence WithSecrets(Secrets secrets)
    {
        Secrets = secrets;
        return this;
    }
    #endregion


    #region Cloning and Merging 

    /// <summary>
    /// Clones the presence into a new instance. Used for thread safe writing and reading. This function will ignore properties if they are in a invalid state.
    /// </summary>
    /// <returns></returns>
    public RichPresence Clone()
    {
        return new RichPresence
        {
            State = this._state != null ? _state.Clone() as string : null,
            Details = this._details != null ? _details.Clone() as string : null,

            Buttons = !HasButtons() ? null : this.Buttons.Clone() as Button[],
            Secrets = !HasSecrets() ? null : new Secrets
            {
                //MatchSecret = this.Secrets.MatchSecret?.Clone() as string,
                JoinSecret = this.Secrets.JoinSecret != null ? this.Secrets.JoinSecret.Clone() as string : null,
                SpectateSecret = this.Secrets.SpectateSecret != null ? this.Secrets.SpectateSecret.Clone() as string : null
            },

            Timestamps = !HasTimestamps() ? null : new Timestamps
            {
                Start = this.Timestamps.Start,
                End = this.Timestamps.End
            },

            Assets = !HasAssets() ? null : new Assets
            {
                LargeImageKey = this.Assets.LargeImageKey != null ? this.Assets.LargeImageKey.Clone() as string : null,
                LargeImageText = this.Assets.LargeImageText != null ? this.Assets.LargeImageText.Clone() as string : null,
                SmallImageKey = this.Assets.SmallImageKey != null ? this.Assets.SmallImageKey.Clone() as string : null,
                SmallImageText = this.Assets.SmallImageText != null ? this.Assets.SmallImageText.Clone() as string : null
            },

            Party = !HasParty() ? null : new Party
            {
                ID = this.Party.ID,
                Size = this.Party.Size,
                Max = this.Party.Max,
                Privacy = this.Party.Privacy,
            },

        };
    }

    /// <summary>
    /// Merges the passed presence with this one, taking into account the image key to image id annoyance.
    /// </summary>
    /// <param name="presence"></param>
    /// <returns>self</returns>
    internal RichPresence Merge(BaseRichPresence presence)
    {
        this._state = presence.State;
        this._details = presence.Details;
        this.Party = presence.Party;
        this.Timestamps = presence.Timestamps;
        this.Secrets = presence.Secrets;

        //If they have assets, we should merge them
        if (presence.HasAssets())
        {
            //Make sure we actually have assets too
            if (!this.HasAssets())
            {
                //We dont, so we will just use theirs
                this.Assets = presence.Assets;
            }
            else
            {
                //We do, so we better merge them!
                this.Assets.Merge(presence.Assets);
            }
        }
        else
        {
            //They dont have assets, so we will just set ours to null
            this.Assets = null;
        }

        return this;
    }

    internal override bool Matches(RichPresence other)
    {
        if (!base.Matches(other)) return false;

        //Check buttons
        if (Buttons == null ^ other.Buttons == null) return false;
        if (Buttons != null)
        {
            if (Buttons.Length != other.Buttons.Length) return false;
            for (int i = 0; i < Buttons.Length; i++)
            {
                var a = Buttons[i];
                var b = other.Buttons[i];
                if (a.Label != b.Label || a.Url != b.Url)
                    return false;
            }
        }

        return true;
    }

    #endregion

    /// <summary>
    /// Operator that converts a presence into a boolean for null checks.
    /// </summary>
    /// <param name="presesnce"></param>
    public static implicit operator bool(RichPresence presesnce)
    {
        return presesnce != null;
    }
}
