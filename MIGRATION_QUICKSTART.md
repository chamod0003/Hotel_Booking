# Quick Start Guide - Apply Discount Migration

## To apply the migration to your database:

### Option 1: Using .NET CLI
Navigate to the solution directory and run:

```bash
cd Infrastructure_Layer
dotnet ef database update --startup-project ..\Presentation_Layer\Presentation_Layer.csproj
```

### Option 2: Using Visual Studio Package Manager Console
1. Open Package Manager Console (Tools > NuGet Package Manager > Package Manager Console)
2. Set Default project to: Infrastructure_Layer
3. Run:
```powershell
Update-Database
```

## Verify Migration

After running the migration, your `HotelRooms` table will have two new columns:
- **Column Name:** DiscountPercentage
  - **Type:** decimal(18,2)
  - **Default Value:** 0
- **Column Name:** FinalDiscountedPrice
  - **Type:** decimal(18,2)
  - **Default Value:** 0
  - **Purpose:** Stores the calculated discounted price in the database

## Test the New Endpoint

### Example 1: Calculate pricing for 1 night
```
GET http://localhost:5000/api/Hotel/1/room-pricing?roomType=0&numberOfNights=1
```

### Example 2: Calculate pricing for multiple nights
```
GET http://localhost:5000/api/Hotel/1/room-pricing?roomType=0&numberOfNights=5
```

Replace:
- `1` with your actual hotel ID
- `0` with the RoomType enum value (0 = Standard, 1 = Deluxe, etc.)
- `5` with the number of nights

## Create/Update Hotels with Discount

When creating or updating hotels, include the discount percentage in the rooms:

```json
POST /api/Hotel
{
  "hotelName": "Luxury Hotel",
  "address": "123 Main Street",
  "latitude": 6.9271,
  "longitude": 79.8612,
  "briefDescription": "A luxury hotel with discounted rooms",
  "rooms": [
    {
      "roomType": 0,
      "totalRooms": 20,
      "pricePerNight": 150.00,
      "discountPercentage": 20.0,
      "maxOccupancy": 2,
      "bedCount": 1,
      "roomSize": 30.0,
      "description": "Standard room with 20% seasonal discount"
    },
    {
      "roomType": 1,
      "totalRooms": 10,
      "pricePerNight": 250.00,
      "discountPercentage": 15.0,
      "maxOccupancy": 3,
      "bedCount": 2,
      "roomSize": 45.0,
      "description": "Deluxe room with 15% discount"
    }
  ]
}
```

## The discount will be automatically calculated and stored in the database!

**What happens automatically:**
1. When you create or update a hotel room with a discount percentage
2. The system calculates: `FinalDiscountedPrice = PricePerNight - (PricePerNight × DiscountPercentage / 100)`
3. This value is **saved to the database** in the `FinalDiscountedPrice` column
4. You can retrieve it anytime without recalculation

**Example:**
- Original Price: $100
- Discount: 15%
- **FinalDiscountedPrice stored in DB**: $85.00
