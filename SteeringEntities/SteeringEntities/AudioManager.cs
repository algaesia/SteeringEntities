using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//When using OpenGL project, Song uses
//SoundEffectInstance
//When using DirectX project, Song works
//as intended (using .wma and .xnb)
namespace SteeringEntities
{
    public class EffectManager
    {
        ContentManager m_Content;
        static EffectManager instance = null;

        Dictionary<string, SoundEffectInstance> m_Effects = new Dictionary<string, SoundEffectInstance>();

        public static EffectManager Instance
        {
            get { return instance == null ? instance = new EffectManager() : instance; }
        }

        private EffectManager()
        {
            m_Content = GameStateManager.Instance.Content;
        }

        //Checks for duplicate key only
        public bool AddEffect(string a_TrackName)
        {
            if (m_Effects.ContainsKey(a_TrackName)) return false;
            m_Effects.Add(a_TrackName, m_Content.Load<SoundEffect>(a_TrackName).CreateInstance());
            return true;
        }

        public bool RemoveEffect(string a_TrackName)
        {
            if (!m_Effects.ContainsKey(a_TrackName)) return false;

            m_Effects.Remove(a_TrackName);
            return true;
        }

        public void PlayEffect(string a_TrackName)
        {
            if (!m_Effects.ContainsKey(a_TrackName)) return;

            m_Effects[a_TrackName].Play();
        }
    }

    public class SongManager
    {
        ContentManager m_Content;
        static SongManager instance = null;

        Dictionary<string, Song> m_Songs = new Dictionary<string, Song>();

        bool switchSongs = false;

        public static SongManager Instance
        {
            get { return instance == null ? instance = new SongManager() : instance; }
        }

        private SongManager()
        {
            m_Content = GameStateManager.Instance.Content;
            MediaPlayer.Volume = 0;
        }

        public bool AddSong(string a_TrackName)
        {
            if (m_Songs.ContainsKey(a_TrackName)) return false;

            Song test = m_Content.Load<Song>(a_TrackName);
            m_Songs.Add(a_TrackName, test);

            return true;
        }

        public bool RemoveSong(string a_TrackName)
        {
            if (!m_Songs.ContainsKey(a_TrackName)) return false;

            m_Songs.Remove(a_TrackName);

            return true;
        }

        public void PlaySong(string a_TrackName)
        {
            if (!switchSongs) switchSongs = true;

            MediaPlayer.Play(m_Songs[a_TrackName]);
        }

        public void PauseSong(string a_TrackName)
        {
            
        }

        //performs fade if required
        public void Update(GameTime gt)
        {
            switch (MediaPlayer.State)
            {
                case MediaState.Playing:
                    if (switchSongs)
                    {
                        if (MediaPlayer.Volume <= 1)
                        {
                            MediaPlayer.Volume += 0.01f;
                        }
                    }
                    break;
                case MediaState.Paused:
                    break;
                case MediaState.Stopped:
                    break;
            }
        }
    }
}
