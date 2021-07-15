namespace LeetCode
{
    public static class Problem2AddTwoNumbers
    {
        public class ListNode
        {
            public int val;
            public ListNode next;
            public ListNode(int val = 0, ListNode next = null)
            {
                this.val = val;
                this.next = next;
            }
        }

        private static readonly ListNode _zero = new();

        public static ListNode AddTwoNumbers(ListNode l1, ListNode l2)
        {
            var first = new ListNode();
            _AddTwoNumbers(first, l1, l2);
            return first;
        }

        private static void _AddTwoNumbers(ListNode current, ListNode one, ListNode two)
        {
            current.val += one.val + two.val;
            int carry;
            if (current.val >= 10)
            {
                current.val -= 10;
                carry = 1;
            }
            else
            {
                carry = 0;
            }
            one = one.next ?? _zero;
            two = two.next ?? _zero;
            if (one != _zero || two != _zero || carry != 0)
            {
                current.next = new(carry);
                _AddTwoNumbers(current.next, one, two);
            }
        }
    }
}
