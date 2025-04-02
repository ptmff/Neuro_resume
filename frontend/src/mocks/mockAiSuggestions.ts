// mocks/mockAiSuggestions.ts

import type { AiSuggestion } from '@/stores/resumesStore'

export interface AiSuggestionsResponse {
  success: boolean;
  resumeId: number;
  suggestions: AiSuggestion[];
  stats: {
    totalSuggestions: number;
    estimatedImprovementScore: number;
    targetPositionMatch: {
      before: number;
      after: number;
    };
  };
}

export const getMockAiSuggestions = (
  resumeId: number = 123,
  targetPosition: string = 'Frontend Developer',
  targetCompany: string = '',
  experienceIds?: string[]
): AiSuggestionsResponse => {
  const targetExperienceId = experienceIds?.[0] || null;

  const suggestions: AiSuggestion[] = [
    {
      "id": "sug-1",
      "type": "experience",
      "title": "Фронтенд-разработчик",
      "description": "Разработал 5 высоконагруженных приложений на React, увеличив конверсию пользователей на 23%.",
      "confidence": 0.9,
      "before": "Разработал 5 высоконагруженных приложений на React, увеличив конверсию пользователей на 23%.",
      "after": "Разработал несколько высоконагруженных приложений на React, внедрив передовые UX/UI решения и увеличив конверсию пользователей на 27%.",
      "reasoning": "Увеличение показателя конверсии на 4% демонстрирует рост навыков и значительное влияние на продукт."
    },
    {
      "id": "sug-2",
      "type": "skills",
      "title": "Skills",
      "description": "Навыки фронтенд-разработки",
      "confidence": 0.8,
      "before": [
        "React",
        "TypeScript",
        "Redux"
      ],
      "after": [
        "React",
        "TypeScript",
        "Redux",
        "Next.js",
        "Chakra UI"
      ],
      "reasoning": "Добавление новых популярных библиотек и фреймворков подчеркнет актуальность ваших навыков и разнообразие вашего опыта."
    }
  ];

  const stats = {
    totalSuggestions: suggestions.length,
    estimatedImprovementScore: 38,
    targetPositionMatch: {
      before: 64,
      after: 88
    }
  };

  return {
    success: true,
    resumeId,
    suggestions,
    stats
  };
};
