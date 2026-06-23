namespace task01;

public static class StringExtensions
{
    public static bool IsPalindrome(this string input)
    {
        if(input.Length == 0) return false;
        
        input = input.ToLower();
        int leftIndex = 0;
        int rightIndex = input.Length - 1;
        while(leftIndex < rightIndex)
        {
            while(leftIndex < rightIndex && (char.IsPunctuation(input[leftIndex]) || char.IsWhiteSpace(input[leftIndex])))
            {
                leftIndex += 1;
            }
            while(leftIndex < rightIndex && (char.IsPunctuation(input[rightIndex]) || char.IsWhiteSpace(input[rightIndex])))
            {
                rightIndex -= 1;
            }

            if(input[leftIndex] != input[rightIndex]) return false;

            leftIndex += 1;
            rightIndex -= 1;
        }
        return true;
    }
}
