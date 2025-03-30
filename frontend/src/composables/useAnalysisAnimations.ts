export function playSound(id: string) {
  const audioMap: Record<string, string> = {
    connect: '/sounds/connect.mp3',
    scanSkills: '/sounds/scan.mp3',
    matchExperience: '/sounds/experience.mp3',
    aiModel: '/sounds/ai.mp3',
    finalize: '/sounds/final.mp3'
  }

  const sound = new Audio(audioMap[id] || '')
  sound.volume = 0.5
  sound.play().catch(() => {
    console.log(`[🔇 fallbackSound] ${id}`)
  })
}

export function showParticles(id: string) {
  const container = document.querySelector(`[data-phase="${id}"]`)
  if (!container) return

  const particle = document.createElement('div')
  particle.className = 'particle-blast'
  container.appendChild(particle)

  setTimeout(() => {
    particle.remove()
  }, 1000)
}

export function animateSection(id: string) {
  const el = document.querySelector(`[data-phase="${id}"]`)
  if (el) {
    el.classList.add('animate-pulse')
    setTimeout(() => el.classList.remove('animate-pulse'), 1000)
  }
}

export function highlightMatchedSkills() {
  const skills = document.querySelectorAll('.matched-skill')
  skills.forEach((el, i) => {
    el.classList.add('animate-bounce')
    setTimeout(() => el.classList.remove('animate-bounce'), 800 + i * 100)
  })
}

export function animateTimeline() {
  const timeline = document.querySelector('.timeline')
  if (timeline) {
    timeline.classList.add('animate-fade-in-up')
    setTimeout(() => timeline.classList.remove('animate-fade-in-up'), 1000)
  }
}
