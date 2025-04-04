// server.js
import express from 'express'
import path from 'path'
import { fileURLToPath } from 'url'
import cors from 'cors'
import fs from 'fs-extra'

const app = express()
const __dirname = path.dirname(fileURLToPath(import.meta.url))
const dataPath = path.join(__dirname, 'data')
const resumesPath = path.join(dataPath, 'resumes.json')
const profilePath = path.join(dataPath, 'profile.json')

app.use(cors({ origin: 'http://localhost:5173', credentials: true }))
app.use(express.json())

// ---------------------- PROFILE ----------------------

app.get('/api/profile', async (req, res) => {
  const profile = await fs.readJson(profilePath)
  res.json(profile)
})

app.patch('/api/profile', async (req, res) => {
  const currentProfile = await fs.readJson(profilePath)
  const updatedProfile = { ...currentProfile, ...req.body }
  await fs.writeJson(profilePath, updatedProfile, { spaces: 2 })
  res.json({ message: 'Profile updated', profile: updatedProfile })
})

app.delete('/api/profile', async (req, res) => {
  try {
    await fs.remove(profilePath);
    res.json({ message: 'Profile deleted' });
  } catch {
    res.status(500).json({ message: 'Error deleting profile' });
  }
});

// ---------------------- AUTH ----------------------

// Регистрация — просто сохраняем профиль
app.post('/api/auth/register', async (req, res) => {
  const newUser = req.body;

  // Если уже есть профиль с таким email
  try {
    const existing = await fs.readJson(profilePath);
    if (existing.email === newUser.email) {
      return res.status(400).json({ message: 'User already exists' });
    }
  } catch {
    // ignore if file doesn't exist
  }

  await fs.writeJson(profilePath, newUser, { spaces: 2 });
  res.json({ message: 'User registered', profile: newUser });
});

// Логин — проверяем email и пароль
app.post('/api/auth/login', async (req, res) => {
  const { email, password } = req.body;

  try {
    const profile = await fs.readJson(profilePath);
    if (profile.email === email && profile.password === password) {
      res.json({ token: 'mock-token-123' }); // заглушка токена
    } else {
      res.status(401).json({ message: 'Invalid credentials' });
    }
  } catch {
    res.status(404).json({ message: 'User not found' });
  }
});

// ---------------------- RESUMES ----------------------

app.get('/api/resumes', async (req, res) => {
  const data = await fs.readJson(resumesPath)
  res.json(data.resumes || [])
})

app.post('/api/resumes', async (req, res) => {
  const data = await fs.readJson(resumesPath)
  const newResume = req.body
  newResume.id = Date.now()
  newResume.date = new Date().toISOString()

  data.resumes.push(newResume)
  await fs.writeJson(resumesPath, data, { spaces: 2 })
  res.json(newResume)
})

app.put('/api/resumes/:id', async (req, res) => {
  const data = await fs.readJson(resumesPath)
  const id = parseInt(req.params.id)
  const updatedResume = req.body
  const index = data.resumes.findIndex(r => r.id === id)

  if (index === -1) return res.status(404).json({ message: 'Resume not found' })

  data.resumes[index] = updatedResume
  await fs.writeJson(resumesPath, data, { spaces: 2 })
  res.json(updatedResume)
})

app.delete('/api/resumes/:id', async (req, res) => {
  const data = await fs.readJson(resumesPath)
  const id = parseInt(req.params.id)
  data.resumes = data.resumes.filter(r => r.id !== id)
  await fs.writeJson(resumesPath, data, { spaces: 2 })
  res.json({ message: `Resume with ID ${id} deleted` })
})

// ---------------------- ANALYSIS ----------------------

// routes/analyse.ts
app.post('/analyse', async (req, res) => {
  const json = await fs.promises.readFile('./data/job-analysis.json', 'utf-8')
  res.json(JSON.parse(json))
})

// ---------------------- FRONTEND ----------------------

const distPath = path.join(__dirname, '../dist')
app.use(express.static(distPath))

app.get('*', (req, res) => {
  res.sendFile(path.join(distPath, 'index.html'))
})

const PORT = 5000
app.listen(PORT, () => {
  console.log(`✅ Сервер запущен на http://localhost:${PORT}`)
})
