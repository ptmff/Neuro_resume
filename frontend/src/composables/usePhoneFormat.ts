export function usePhoneFormat() {
    /**
     * Formats a phone number to the pattern: +7 (XXX) XXX XX-XX
     * Handles inputs like: 89105974723, +79105974723, 79105974723
     * Works in real-time as the user types
     */
    const formatPhoneNumber = (input: string): string => {
      if (!input) return ""
  
      // Remove all non-digit characters except the leading +
      let formatted = input.replace(/[^\d+]/g, "")
  
      // Handle the + sign - only keep it at the beginning
      if (formatted.startsWith("+")) {
        formatted = "+" + formatted.substring(1).replace(/\+/g, "")
      } else {
        formatted = formatted.replace(/\+/g, "")
      }
  
      // Extract just the digits
      const digits = formatted.replace(/\D/g, "")
  
      // Handle Russian numbers
      if (digits.length > 0) {
        // If starts with 8, replace with 7
        if (digits.startsWith("8") && digits.length > 1) {
          formatted = "+7" + digits.substring(1)
        }
        // If starts with 7 or already has +7, ensure proper format
        else if (digits.startsWith("7")) {
          if (formatted.startsWith("+")) {
            formatted = "+7" + digits.substring(1)
          } else {
            formatted = "+7" + digits.substring(1)
          }
        }
        // If doesn't start with country code but has enough digits for a Russian number
        else if (digits.length >= 10 && !formatted.startsWith("+")) {
          formatted = "+7" + digits
        }
        // Keep as is for partial input or international numbers
        else if (!formatted.startsWith("+") && digits.length > 0) {
          formatted = "+7" + digits
        }
      }
  
      // Format according to the pattern: +7 (XXX) XXX XX-XX
      if (formatted.startsWith("+7")) {
        const remainingDigits = formatted.substring(2).replace(/\D/g, "")
  
        // Build the formatted string incrementally
        let result = "+7"
  
        // Add area code parentheses (XXX)
        if (remainingDigits.length > 0) {
          result += " ("
          result += remainingDigits.substring(0, Math.min(3, remainingDigits.length))
  
          if (remainingDigits.length > 3) {
            result += ") "
            result += remainingDigits.substring(3, Math.min(6, remainingDigits.length))
  
            if (remainingDigits.length > 6) {
              result += " "
              result += remainingDigits.substring(6, Math.min(8, remainingDigits.length))
  
              if (remainingDigits.length > 8) {
                result += "-"
                result += remainingDigits.substring(8, Math.min(10, remainingDigits.length))
              }
            }
          } else if (remainingDigits.length === 3) {
            result += ")"
          }
        }
  
        return result
      }
  
      return formatted
    }
  
    /**
     * Calculates the cursor position after formatting
     */
    const calculateCursorPosition = (
      previousValue: string,
      currentValue: string,
      previousPosition: number | null,
    ): number => {
      if (previousPosition === null) return currentValue.length
  
      // Count special characters before cursor in the previous value
      const prevSpecialBefore = (previousValue.substring(0, previousPosition).match(/[^\d]/g) || []).length
  
      // Count digits before cursor in the previous value
      const prevDigitsBefore = previousPosition - prevSpecialBefore
  
      // Count special characters in the new value up to the same number of digits
      let digitCount = 0
      let newPosition = 0
  
      for (let i = 0; i < currentValue.length; i++) {
        if (/\d/.test(currentValue[i])) {
          digitCount++
        }
        if (digitCount > prevDigitsBefore) break
        newPosition = i + 1
      }
  
      return newPosition
    }
  
    /**
     * Normalizes a formatted phone number back to E.164 format (+7XXXXXXXXXX)
     * for storage or API calls
     */
    const normalizePhoneNumber = (formattedPhone: string): string => {
      const digits = formattedPhone.replace(/\D/g, "")
  
      if (digits.length === 11 && (digits.startsWith("7") || digits.startsWith("8"))) {
        return "+7" + digits.substring(1)
      }
  
      return formattedPhone
    }
  
    return {
      formatPhoneNumber,
      calculateCursorPosition,
      normalizePhoneNumber,
    }
  }
  
  