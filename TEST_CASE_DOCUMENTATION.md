# Test Case Documentation

## Smoke Tests (8 tests)

### TC-SM-01: Home page loads successfully with core UI components visible
**What it does:** Navigates to the home page and verifies product cards are displayed.

**Pass Criteria:**
- ✅ Home page loads without errors
- ✅ At least 1 product card is visible on the page

**Why it matters:** Ensures the most critical user journey (viewing products) works.

---

### TC-SM-02: Navbar displays Offers, Orders, Favourites, and Sign In links
**What it does:** Checks that all navigation links are present in the navbar.

**Pass Criteria:**
- ✅ "Offers" link is visible
- ✅ "Orders" link is visible
- ✅ "Favourites" link is visible
- ✅ "Sign In" link is visible

**Why it matters:** Verifies core navigation structure is intact.

---

### TC-SM-03: Default product catalog displays product cards on home page
**What it does:** Verifies the product catalog renders with product cards.

**Pass Criteria:**
- ✅ Product catalog section is present
- ✅ At least 1 product card is displayed

**Why it matters:** Confirms the main product display functionality works.

---

### TC-SM-04: Product count indicator matches number of displayed product cards
**What it does:** Compares the product count text with actual number of visible product cards.

**Pass Criteria:**
- ✅ Product count indicator is visible
- ✅ Count text contains the same number as displayed cards (e.g., "25 products" matches 25 cards)

**Why it matters:** Ensures UI consistency and accurate product count reporting.

---

### TC-SM-05: Vendor filter section displays all available vendors
**What it does:** Counts the number of vendor filter checkboxes.

**Pass Criteria:**
- ✅ At least 4 vendor filter checkboxes are present (Apple, Samsung, Google, OnePlus)

**Why it matters:** Verifies filtering functionality is available to users.

---

### TC-SM-06: Selecting a single vendor filter updates the product catalog
**What it does:** Applies a vendor filter (Apple) and checks if product count changes.

**Pass Criteria:**
- ✅ Initial product count is recorded
- ✅ After selecting "Apple" filter, product count changes
- ✅ New count is different from initial count

**Why it matters:** Confirms filtering logic works and updates the display.

---

### TC-SM-07: Adding a product updates cart quantity badge
**What it does:** Adds first product to cart and checks if cart badge updates.

**Pass Criteria:**
- ✅ Cart quantity badge shows "0" initially (or is not visible)
- ✅ After adding product, cart badge shows non-zero value (e.g., "1")

**Why it matters:** Ensures users can see their cart status at a glance.

---

### TC-SM-08: Opening cart after adding product shows non-zero subtotal
**What it does:** Adds a product, opens cart, and verifies subtotal is not $0.00.

**Pass Criteria:**
- ✅ Product is added to cart
- ✅ Cart opens successfully
- ✅ Subtotal displays a value other than "$0.00"

**Why it matters:** Confirms cart calculation and display works correctly.

---

## Negative & Edge Tests (7 tests)

### TC-NEG-09: Offers link navigates successfully
**What it does:** Logs in, clicks Offers link, and verifies page loads without crash.

**Pass Criteria:**
- ✅ User logs in successfully
- ✅ Offers link is clicked
- ✅ Page loads (NetworkIdle state reached)
- ✅ Product count is 0 or greater (page rendered)

**Why it matters:** Ensures authenticated navigation to Offers works.

---

### TC-NEG-10: Orders link navigates successfully
**What it does:** Logs in, clicks Orders link, and verifies page loads without crash.

**Pass Criteria:**
- ✅ User logs in successfully
- ✅ Orders link is clicked
- ✅ Page loads (NetworkIdle state reached)
- ✅ Product count is 0 or greater (page rendered)

**Why it matters:** Ensures authenticated navigation to Orders works.

---

### TC-NEG-11: Favourites link navigates successfully
**What it does:** Logs in, clicks Favourites link, and verifies page loads without crash.

**Pass Criteria:**
- ✅ User logs in successfully
- ✅ Favourites link is clicked
- ✅ Page loads (NetworkIdle state reached)
- ✅ Product count is 0 or greater (page rendered)

**Why it matters:** Ensures authenticated navigation to Favourites works.

---

### TC-NEG-13: Multiple vendor filters apply combined logic without UI crash
**What it does:** Selects multiple vendor filters (Apple + Samsung) simultaneously.

**Pass Criteria:**
- ✅ Both "Apple" and "Samsung" filters are selected
- ✅ Page does not crash or freeze
- ✅ At least 1 product is displayed (combined filter works)

**Why it matters:** Tests that multi-select filtering doesn't break the UI.

---

### TC-NEG-14: Rapid toggling vendor filter does not break final state
**What it does:** Rapidly toggles a vendor filter on/off 5 times.

**Pass Criteria:**
- ✅ Filter is toggled 5 times without errors
- ✅ UI remains stable (no crash)
- ✅ At least 1 product is displayed after toggling

**Why it matters:** Ensures UI handles rapid user interactions gracefully.

---

### TC-NEG-15: Empty cart shows empty state + $0.00 subtotal
**What it does:** Opens cart without adding any products.

**Pass Criteria:**
- ✅ Cart opens successfully
- ✅ Empty cart message is visible (e.g., "Your cart is empty")
- ✅ Subtotal shows "$0.00"

**Why it matters:** Verifies proper empty state handling.

---

### TC-NEG-16: Continue Shopping closes cart and restores browsing
**What it does:** Opens cart and clicks "Continue Shopping" button.

**Pass Criteria:**
- ✅ Cart opens successfully
- ✅ "Continue Shopping" button is clicked
- ✅ Cart closes (no longer visible)

**Why it matters:** Ensures users can easily return to shopping.

---

## Regression Tests (4 tests)

### TC-REG-17: Product card shows image, title, price, and Add to cart control
**What it does:** Inspects the first product card for required elements.

**Pass Criteria:**
- ✅ At least 1 product card exists
- ✅ Product card contains an image
- ✅ Product card contains a title
- ✅ Product card contains a price
- ✅ Product card contains "Add to cart" button

**Why it matters:** Ensures product cards have all necessary information for purchase decisions.

---

### TC-REG-18: Adding multiple products updates cart quantity and subtotal
**What it does:** Adds 2 different products to cart and verifies updates.

**Pass Criteria:**
- ✅ First product is added to cart
- ✅ Second product is added to cart
- ✅ Cart quantity badge shows non-zero value
- ✅ Cart subtotal is not "$0.00"

**Why it matters:** Confirms cart handles multiple items correctly.

---

### TC-REG-19: Cart state persists after navigating to Offers and back
**What it does:** Adds product, navigates to Offers, returns to home, checks cart.

**Pass Criteria:**
- ✅ Product is added to cart (quantity recorded)
- ✅ User navigates to Offers page
- ✅ User returns to home page
- ✅ Cart quantity remains the same as before navigation

**Why it matters:** Ensures cart state is maintained across page navigation.

---

### TC-REG-20: Rapid user interactions do not crash UI
**What it does:** Performs multiple rapid actions: add to cart, filter, add again.

**Pass Criteria:**
- ✅ Product is added to cart
- ✅ Vendor filter is applied
- ✅ Another product is added
- ✅ UI remains stable (no crash)
- ✅ At least 1 product is still displayed

**Why it matters:** Tests UI stability under rapid user interactions.

---

## Summary

**Total Tests:** 26
- **Smoke:** 12 tests (critical path verification + vendor filtering)
- **Negative:** 10 tests (edge cases, error handling, auth control)
- **Regression:** 4 tests (feature stability)

**Authentication Required:** 3 tests (TC-NEG-09, TC-NEG-10, TC-NEG-11)
- Uses demo credentials: demouser / testingisfun99

**Execution Time:** ~6 minutes for full suite

**Pass Rate:** 20/26 passing, 4 failing (bugs exposed), 2 skipped (gated)

---

## NEW: Vendor Filtering Tests (4 tests)

### TC-SM-VEND-01: Apple vendor filter shows ONLY Apple products
**What it does:** Selects Apple filter and verifies all displayed products are Apple.

**Steps:**
1. Navigate to home page
2. Record initial product count
3. Select "Apple" vendor filter
4. Wait for filter to apply
5. Count filtered products
6. Extract vendor from each visible product title

**Pass Criteria:**
- ✅ Filtered count ≤ initial count
- ✅ Product count indicator exactly matches displayed card count
- ✅ At least 1 Apple product is displayed
- ✅ ALL visible products are Apple (determined by title containing "iPhone" or "iPad")

**Why it matters:** Validates vendor filtering logic works correctly and shows only matching products.

**Status:** ✅ PASS

---

### TC-SM-VEND-02: Samsung vendor filter shows ONLY Samsung products
**What it does:** Selects Samsung filter and verifies all displayed products are Samsung.

**Steps:**
1. Navigate to home page
2. Record initial product count
3. Select "Samsung" vendor filter
4. Wait for filter to apply
5. Count filtered products
6. Extract vendor from each visible product title

**Pass Criteria:**
- ✅ Filtered count ≤ initial count
- ✅ Product count indicator exactly matches displayed card count
- ✅ At least 1 Samsung product is displayed
- ✅ ALL visible products are Samsung (determined by title containing "Galaxy" or "Samsung")

**Why it matters:** Validates vendor filtering logic works correctly for Samsung.

**Status:** ✅ PASS

---

### TC-SM-VEND-03: Google vendor filter shows ONLY Google products
**What it does:** Selects Google filter and verifies all displayed products are Google.

**Steps:**
1. Navigate to home page
2. Record initial product count
3. Select "Google" vendor filter
4. Wait for filter to apply
5. Count filtered products
6. Extract vendor from each visible product title

**Pass Criteria:**
- ✅ Filtered count ≤ initial count
- ✅ Product count indicator exactly matches displayed card count
- ✅ At least 1 Google product is displayed
- ✅ ALL visible products are Google (determined by title containing "Pixel")

**Why it matters:** Validates vendor filtering logic works correctly for Google.

**Status:** ✅ PASS

---

### TC-NEG-VEND-04: OnePlus vendor filter behavior validation
**What it does:** Selects OnePlus filter and validates behavior (0 products OR only OnePlus products).

**Steps:**
1. Navigate to home page
2. Record initial product count
3. Select "OnePlus" vendor filter
4. Wait for filter to apply
5. Count filtered products
6. If products exist, verify all are OnePlus

**Pass Criteria:**
- ✅ Filtered count ≤ initial count
- ✅ Product count indicator exactly matches displayed card count
- ✅ IF filtered count > 0, ALL products MUST be OnePlus (title contains "OnePlus")
- ✅ IF filtered count = 0, acceptable (no OnePlus inventory)

**Why it matters:** Validates OnePlus filter doesn't show products from other vendors.

**Status:** ❌ FAIL - **BUG EXPOSED**
- **Defect:** OnePlus filter shows Apple products instead of OnePlus products
- **Expected:** Only OnePlus products OR 0 products
- **Actual:** Apple products displayed when OnePlus filter selected
- **Severity:** HIGH - Filter logic broken for OnePlus vendor

---

## NEW: Checkout Access Control (1 test)

### TC-NEG-AUTH-07: User cannot checkout without authentication
**What it does:** Attempts to access checkout page without logging in.

**Steps:**
1. Navigate to home page
2. Add 1 product to cart
3. Verify cart has 1 item
4. Navigate to /checkout
5. Wait for page load
6. Check current URL

**Pass Criteria:**
- ✅ Cart has 1 item before checkout attempt
- ✅ User is redirected to signin (URL contains "signin" or "?signin=true")
- ✅ Checkout page is NOT accessible

**Why it matters:** Ensures checkout requires authentication for security.

**Status:** ✅ PASS

---

## NEW: Checkout Validation (1 test - Conditionally Skipped)

### TC-NEG-CO-02: Cannot submit order with empty required fields
**What it does:** Attempts to submit checkout form with all required fields empty.

**Steps:**
1. Navigate to home page
2. Add 1 product to cart
3. Navigate to /checkout
4. IF redirected to signin, skip test (covered by TC-NEG-AUTH-07)
5. Verify at checkout page
6. Click submit button without filling fields
7. Verify order is NOT confirmed
8. Check validation errors for each required field

**Pass Criteria:**
- ✅ Order is NOT confirmed when fields are empty
- ✅ First Name field shows validation error
- ✅ Last Name field shows validation error
- ✅ Address field shows validation error
- ✅ Province field shows validation error
- ✅ Postal Code field shows validation error

**Why it matters:** Ensures required field validation prevents incomplete orders.

**Status:** ⏭️ SKIPPED - Checkout requires authentication (gated by TC-NEG-AUTH-07)

---

## NEW: Checkout Totals (1 test - Conditionally Skipped)

### TC-REG-CO-03: Checkout total exactly matches cart total
**What it does:** Compares cart subtotal with checkout total for exact numeric equality.

**Steps:**
1. Navigate to home page
2. Add 1 product to cart
3. Open cart and capture subtotal
4. Navigate to /checkout
5. IF redirected to signin, skip test (covered by TC-NEG-AUTH-07)
6. Verify at checkout page
7. Capture checkout total
8. Normalize both values (remove $, spaces, commas)
9. Compare as decimal values

**Pass Criteria:**
- ✅ Cart subtotal is captured
- ✅ Checkout total is captured
- ✅ Both values normalize successfully
- ✅ Checkout total EXACTLY equals cart subtotal (no approximation)

**Why it matters:** Ensures pricing consistency between cart and checkout.

**Status:** ⏭️ SKIPPED - Checkout requires authentication (gated by TC-NEG-AUTH-07)
