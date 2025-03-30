// mocks/mockAnalysis.ts
export const fetchMockAnalysis = async (jobText: string) => {
    await new Promise(resolve => setTimeout(resolve, 1000))
    return {
      result: {
        matchScore: 84,
        missingSkills: ['Docker', 'Kubernetes'],
        overlappingExperience: true,
        recommendations: ['Добавьте опыт с Docker', 'Уточните опыт в DevOps'],
      },
      steps: [
        { type: 'skills', description: 'Совпадения по навыкам найдены' },
        { type: 'gap', description: 'Пробелы в опыте обнаружены' },
        { type: 'ai', description: 'AI сформировал итоговую оценку' },
      ],
    }
  }
  