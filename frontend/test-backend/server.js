import express from 'express'
import path from 'path'
import { fileURLToPath } from 'url'
import cors from 'cors'
import fs from 'fs-extra'

const app = express()
const __dirname = path.dirname(fileURLToPath(import.meta.url))
const dataPath = path.join(__dirname, 'data')

app.use(cors({
  origin: 'http://localhost:5173',
  credentials: true
}))
app.use(express.json())

const distPath = path.join(__dirname, '../dist')
app.use(express.static(distPath))

app.get('/api/profile', async (req, res) => {
  const profile = await fs.readJson(path.join(dataPath, 'profile.json'))
  res.json(profile)
})

app.get('/api/resumes', async (req, res) => {
  const resumes = await fs.readJson(path.join(dataPath, 'resumes.json'))
  res.json(resumes.resumes)
})

app.patch('/api/profile', async (req, res) => {
  const profilePath = path.join(dataPath, 'profile.json')
  const currentProfile = await fs.readJson(profilePath)

  const updatedProfile = {
    ...currentProfile,
    ...req.body
  }

  console.log('PATCH /api/profile body:', req.body)
  console.log('Обновляем профиль:', updatedProfile)

  await fs.writeJson(profilePath, updatedProfile, { spaces: 2 })
  res.json({ message: 'Profile updated', profile: updatedProfile })
})

app.patch('/api/resumes', async (req, res) => {
  const resumesPath = path.join(dataPath, 'resumes.json')
  const { resumes } = req.body

  if (!Array.isArray(resumes)) {
    return res.status(400).json({ message: 'Invalid format' })
  }

  await fs.writeJson(resumesPath, { resumes }, { spaces: 2 })
  res.json({ message: 'Resumes updated', resumes })
})

app.get('*', (req, res) => {
  res.sendFile(path.join(distPath, 'index.html'))
})

const PORT = 5000
app.listen(PORT, () => {
  console.log(`✅ Сервер запущен на http://localhost:${PORT}`)
})
