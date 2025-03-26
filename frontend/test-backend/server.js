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
