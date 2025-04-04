import { defineStore } from "pinia"
import { ref, computed, watch } from "vue"
import api from "@/api"
import type { Resume, AiSuggestion, AiStats, AiSuggestionsResponse } from "@/types/types"
import { useProfileStore } from "./profileStore"

const LOCAL_STORAGE_KEY = "resumeToEdit"

export const useResumeStore = defineStore("resumes", () => {
  const resumes = ref<Resume[]>([])
  const resumeToEdit = ref<Resume | null>(null)
  const aiSuggestions = ref<AiSuggestion[]>([])
  const aiStats = ref<AiStats | null>(null)
  const isAnalyzing = ref(false)
  const analyzeTimeout = ref<number | null>(null)

  const profileStore = useProfileStore() // Initialize profileStore here

  // Add storage for applied/ignored suggestions that persists during navigation
  // Add these new state variables after the existing ones:

  const appliedSuggestionIds = ref<string[]>([])
  const ignoredSuggestionIds = ref<string[]>([])

  // Watch for changes to resumeToEdit and save to localStorage
  watch(
    resumeToEdit,
    (newValue) => {
      if (newValue) {
        localStorage.setItem(LOCAL_STORAGE_KEY, JSON.stringify(newValue))
      } else {
        localStorage.removeItem(LOCAL_STORAGE_KEY)
      }
    },
    { deep: true },
  ) // Deep watch to catch nested property changes

  const fetchResumes = async () => {
    try {
      const response = await api.get("/Resumes")
      resumes.value = response.data
      console.log("[resumesStore] resumes loaded:", resumes.value)
    } catch (err) {
      console.error("[resumesStore] error loading resumes:", err)
    }
  }

  const addResume = async (newResume: Resume) => {
    try {
      const response = await api.post("/Resumes", newResume)
      resumes.value.push(response.data)

      profileStore.setMainResume(response.data.id)
      await profileStore.updateProfile({ mainResumeId: response.data.id })
    } catch (err) {
      console.error("[resumesStore] error adding resume:", err)
    }
  }

  const updateResume = async (updatedResume: Resume) => {
    try {
      await api.put(`/Resumes/${updatedResume.id}`, updatedResume)
      const index = resumes.value.findIndex((r: Resume) => r.id === updatedResume.id)
      if (index !== -1) resumes.value[index] = updatedResume

      // Also update resumeToEdit if it's the same resume
      if (resumeToEdit.value && resumeToEdit.value.id === updatedResume.id) {
        resumeToEdit.value = { ...updatedResume }
      }
    } catch (err) {
      console.error("[resumesStore] error updating resume:", err)
    }
  }

  const deleteResume = async (id: number) => {
    try {
      await api.delete(`/Resumes/${id}`)
      resumes.value = resumes.value.filter((r: Resume) => r.id !== id)

      if (profileStore.profile?.mainResumeId === id) {
        profileStore.setMainResume(null)
        await profileStore.updateProfile({ mainResumeId: null })
      }

      // Clear resumeToEdit if it's the deleted resume
      if (resumeToEdit.value && resumeToEdit.value.id === id) {
        setResumeForEdit(null)
      }
    } catch (err) {
      console.error("[resumesStore] error deleting resume:", err)
    }
  }

  const setResumeForEdit = (resume: Resume | null) => {
    resumeToEdit.value = resume ? { ...resume } : null
    // localStorage saving is handled by the watcher
  }

  const restoreResumeToEdit = () => {
    const saved = localStorage.getItem(LOCAL_STORAGE_KEY)
    if (saved) {
      try {
        resumeToEdit.value = JSON.parse(saved)
      } catch (err) {
        console.warn("[resumesStore] failed to parse saved resume:", err)
        localStorage.removeItem(LOCAL_STORAGE_KEY)
      }
    }
  }

  // Update the applySuggestion function to store the applied suggestion ID:
  const applySuggestion = (suggestion: AiSuggestion) => {
    if (!resumeToEdit.value) return

    const updated = { ...resumeToEdit.value }

    switch (suggestion.type) {
      case "title":
        updated.title = suggestion.after as string
        break

      case "skills":
        if (Array.isArray(suggestion.after)) {
          updated.skills = [...suggestion.after]
        }
        break

      case "experience":
        if (Array.isArray(suggestion.after)) {
          // Handle case where entire experience array is replaced
          updated.experience = [...suggestion.after]
        } else if (suggestion.targetExperienceId) {
          // Handle case where a specific experience entry is updated
          const index = updated.experience.findIndex((e) => e.id === suggestion.targetExperienceId)
          if (index !== -1) {
            updated.experience[index] = {
              ...updated.experience[index],
              ...(typeof suggestion.after === "object"
                ? suggestion.after
                : { description: suggestion.after as string }),
            }
          }
        } else if (updated.experience.length > 0) {
          // Fallback to updating the first experience entry
          updated.experience[0] = {
            ...updated.experience[0],
            ...(typeof suggestion.after === "object" ? suggestion.after : { description: suggestion.after as string }),
          }
        }
        break

      case "description":
        updated.description = suggestion.after as string
        break

      case "education":
        if (Array.isArray(suggestion.after)) {
          updated.education = [...suggestion.after]
        }
        break
    }

    resumeToEdit.value = updated

    // Store the applied suggestion ID
    appliedSuggestionIds.value.push(suggestion.id)

    // Remove the applied suggestion
    aiSuggestions.value = aiSuggestions.value.filter((s) => s.id !== suggestion.id)

    // Update stats if available
    if (aiStats.value) {
      aiStats.value.totalSuggestions--
    }
  }

  // Add a function to handle ignored suggestions:
  const ignoreSuggestion = (suggestionId: string) => {
    // Store the ignored suggestion ID
    ignoredSuggestionIds.value.push(suggestionId)

    // Remove the suggestion from the list
    aiSuggestions.value = aiSuggestions.value.filter((s) => s.id !== suggestionId)

    // Update stats if available
    if (aiStats.value) {
      aiStats.value.totalSuggestions--
    }
  }

  // Add a function to clear suggestion history (to be called when saving the resume):
  const clearSuggestionHistory = () => {
    appliedSuggestionIds.value = []
    ignoredSuggestionIds.value = []
  }

  // Modify the analyzeResume function to filter out already applied or ignored suggestions:
  const analyzeResume = async (resume: Resume) => {
    isAnalyzing.value = true

    // Clear any existing timeout
    if (analyzeTimeout.value) {
      clearTimeout(analyzeTimeout.value)
      analyzeTimeout.value = null
    }

    try {
      const { id, ...resumeWithoutId } = resume

      // Set a long timeout (5 minutes) to ensure we wait for the response
      const timeoutPromise = new Promise<never>((_, reject) => {
        analyzeTimeout.value = window.setTimeout(
          () => {
            reject(new Error("Resume analysis timeout after 5 minutes"))
          },
          5 * 60 * 1000,
        )
      })

      // Race the API call against the timeout
      const response = await Promise.race([
        api.post<AiSuggestionsResponse | AiSuggestion[]>("/ResumeAnalysis/analyze", resumeWithoutId),
        timeoutPromise,
      ])

      // Clear the timeout since we got a response
      if (analyzeTimeout.value) {
        clearTimeout(analyzeTimeout.value)
        analyzeTimeout.value = null
      }

      let newSuggestions: AiSuggestion[] = []

      // Handle different response formats
      if (Array.isArray(response.data)) {
        newSuggestions = response.data
        aiStats.value = null
      } else if (response.data && typeof response.data === "object") {
        if ("suggestions" in response.data && Array.isArray(response.data.suggestions)) {
          newSuggestions = response.data.suggestions
          aiStats.value = "stats" in response.data ? response.data.stats : null
        } else {
          // Fallback if the structure is unexpected
          newSuggestions = Array.isArray(response.data) ? response.data : []
          aiStats.value = null
        }
      }

      // Filter out suggestions that have already been applied or ignored
      aiSuggestions.value = newSuggestions.filter(
        (suggestion) =>
          !appliedSuggestionIds.value.includes(suggestion.id) && !ignoredSuggestionIds.value.includes(suggestion.id),
      )

      // Update stats if needed
      if (aiStats.value && aiStats.value.totalSuggestions !== aiSuggestions.value.length) {
        aiStats.value.totalSuggestions = aiSuggestions.value.length
      }
    } catch (err) {
      console.error("[resumesStore] error during AI resume analysis:", err)
    } finally {
      isAnalyzing.value = false
    }
  }

  // Initialize by restoring from localStorage
  restoreResumeToEdit()

  const mainResume = computed(() => {
    const profile = profileStore.profile
    if (!profile || !profile.mainResumeId) return null
    return resumes.value.find((r: Resume) => r.id === profile.mainResumeId) || null
  })

  // Update the return statement to include the new functions:
  return {
    resumes,
    resumeToEdit,
    aiSuggestions,
    aiStats,
    isAnalyzing,
    appliedSuggestionIds,
    ignoredSuggestionIds,
    fetchResumes,
    addResume,
    updateResume,
    deleteResume,
    setResumeForEdit,
    restoreResumeToEdit,
    mainResume,
    analyzeResume,
    applySuggestion,
    ignoreSuggestion,
    clearSuggestionHistory,
  }
})

