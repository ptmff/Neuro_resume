// mocks/mockAnalysis.ts
export const fetchMockAnalysis = async (jobText: string) => {
  await new Promise(resolve => setTimeout(resolve, 1500))

  return {
    result: {
      matchScore: 78,
      missingSkills: ['TypeScript', 'GraphQL', 'Docker'],
      overlappingExperience: true,
      recommendations: [
        'Добавьте больше описания в последний опыт работы',
        'Укажите конкретные проекты с Vue',
        'Подчеркните опыт с REST API'
      ]
    }
  }
}
