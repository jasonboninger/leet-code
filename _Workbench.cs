using System;
using System.Collections.Generic;

public class ListNode {
    public int val;
    public ListNode next;
    public ListNode(int val=0, ListNode next=null) {
        this.val = val;
        this.next = next;
    }
}

public class Solution
{
    private static readonly ListNode _zero = new ListNode() { next = _zero };

    public ListNode AddTwoNumbers(ListNode l1, ListNode l2)
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
            current.next = new ListNode(carry);
            _AddTwoNumbers(current.next, one, two);
        }
    }
}
